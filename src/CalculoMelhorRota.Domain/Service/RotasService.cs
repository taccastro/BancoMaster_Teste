using CalculoMelhorRota.CrossCutting.Util.Configs;
using CalculoMelhorRota.Domain.Entity;
using CalculoMelhorRota.Domain.Interfaces;
using CalculoMelhorRota.Domain.Validation;
using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;

namespace CalculoMelhorRota.Domain.Service
{
    public class RotasService : BaseService, IRotasService
    {
        private readonly IRotasRepository _repository;
        public RotasService(INotifier notifier, IRotasRepository repository) : base(notifier)
        {
            _repository = repository;
        }

        public IEnumerable<Rotas> GetRotas()
        {
            List<Rotas> rotas = _repository.GetRotas();
            return rotas;
        }

        public IEnumerable<Rotas> AdicionarRotas(IEnumerable<Rotas> rotas)
        {
            //Valida se os dados estao corretos
            var resulValidation = new RotasCollectionValidator();
            ValidationResult results = resulValidation.Validate(rotas);

            if (!results.IsValid)
            {
                foreach (var failure in results.Errors)
                {
                    Notification(failure.ErrorMessage);
                }
                return new List<Rotas>();
            }

            List<Rotas> rotasInseridas = _repository.GetRotas();

            rotasInseridas.AddRange(rotas);
            //Remove duplicidades
            rotasInseridas = rotasInseridas.GroupBy(x => new { x.Origem, x.Destino }).Select(o => o.First()).ToList();
            _repository.Insert(rotasInseridas);

            return rotas;
        }

        public string MelhorRota(string srcRotas)
        {
            //Validação das entradas
            if (srcRotas.Split('-').Length != 2)
            {
                Notification("Rota deve seguir o padrão ex:(GRU-SCL)");
                return null;
            }

            var origem = srcRotas.Split('-')[0];
            var destino = srcRotas.Split('-')[1];

            //Retorna as rotas do CSV
            List<Rotas> rotas = _repository.GetRotas();
            if (!rotas.Any())
            {
                Notification(@$"Arquivo *.csv não carregado");
                Notification(@$"Por favor adicione o arquivo rotas.csv(solution) e crie uma pasta na unidade c: 'C:\App_Rotas\");
                return null;
            }
            //Filtra todas as origens possiveis
            var origens = rotas.Where(x => x.Origem.ToLower() == origem.ToLower()).ToList();
            //Valida se o Destino e origem existe
            if (!origens.Any())
            {
                Notification("Rota de origem Inválida.");
                return null;
            }
            if (!rotas.Where(x => x.Destino.ToLower() == destino.ToLower()).Any())
            {
                Notification("Rota de destino Inválida.");
                return null;
            }

            Resultado resultadoFinal = null;
            List<Resultado> resultado = new List<Resultado>();
            //Varre todas as origens possiveis
            for (var k = 0; k < origens.Count(); ++k)
            {
                var resultadoCalculo = CalculoRota(rotas, origem, destino, origens[k].Destino, origens[k].Destino, origens[k].Valor);
                //Valida se o calculo teve algum sucesso
                if (resultadoCalculo != null && (resultadoFinal == null || resultadoFinal.Valor > resultadoCalculo.Valor))
                {
                    resultadoFinal = resultadoCalculo;
                }
            }


            string strResultado = "";
            if (resultadoFinal == null)
                strResultado = "Não foi possivel calcular uma rota.";
            else
                strResultado = "Melhor Rota: " + resultadoFinal.Rota + " ao custo de R$:" + resultadoFinal.Valor;

            return strResultado;
        }

        Resultado CalculoRota(List<Rotas> rotas, string origem, string destinoFinal, string destinoCompleto, string destinoAtual, int valorRotaAtual)
        {
            //Pega as origens q possuem o destino do contexto
            var rotasDestinoAtual = rotas.Where(x => x.Origem == destinoAtual && x.Destino != origem).OrderBy(x => x.Valor).ToList();

            Resultado resultadoFinal;

            if (rotasDestinoAtual.Any() && destinoAtual != destinoFinal && !destinoCompleto.Contains(destinoFinal))
            {
                string rotaFinal = $@"{destinoCompleto} - {rotasDestinoAtual[0].Destino}";
                int valorRotaFinal = rotasDestinoAtual[0].Valor + valorRotaAtual;

                if (rotasDestinoAtual.Count == 1)
                {
                    resultadoFinal = CalculoRota(rotas, origem, destinoFinal, rotaFinal, rotasDestinoAtual[0].Destino, valorRotaFinal);
                }
                else
                {
                    var calculosRota = new List<Resultado>();
                    foreach (var item in rotasDestinoAtual)
                    {
                        rotaFinal = $@"{destinoCompleto} - {item.Destino}";
                        valorRotaFinal = item.Valor + valorRotaAtual;
                        var resultadoCalculoRotas = CalculoRota(rotas, origem, destinoFinal, rotaFinal, item.Destino, valorRotaFinal);
                        if (resultadoCalculoRotas != null)//adiciona apenas os que retornarem diferente de null
                            calculosRota.Add(resultadoCalculoRotas);
                    }
                    //Pega a melhor rota
                    resultadoFinal = calculosRota.OrderBy(x => x.Valor).FirstOrDefault();
                }

            }
            else
            {
                //Caso for nullo valida se possui o destino
                if (!rotasDestinoAtual.Any() && !destinoCompleto.Contains(destinoFinal))
                    return null;


                string rotaFinal = $@"{origem} - {destinoCompleto}";
                resultadoFinal = new Resultado
                {
                    Rota = rotaFinal,
                    Valor = valorRotaAtual
                };
            }

            return resultadoFinal;
        }
    }
}
