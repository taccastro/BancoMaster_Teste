using CalculoMelhorRota.CrossCutting.Util.Configs;
using CalculoMelhorRota.Domain.Entity;
using CalculoMelhorRota.Domain.Interfaces;
using CalculoMelhorRotaConsole.Interfaces;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculoMelhorRotaConsole.Service
{
    public class AppService : IAppService
    {
        private readonly IRotasService _rotaService;
        private readonly INotifier _notifier;

        public AppService(INotifier notifier, IRotasService rotasService)
        {
            _rotaService = rotasService;
            _notifier = notifier;
        }
        public void ExecutaCalculoRota(string pathCSV)
        {
            var rotas = new List<Rotas>();

            using (TextFieldParser csvParser = new TextFieldParser(pathCSV))
            {
                csvParser.TextFieldType = FieldType.Delimited;
                csvParser.SetDelimiters(",");

                while (!csvParser.EndOfData)
                {
                    string[] fields = csvParser.ReadFields();
                    rotas.Add(new Rotas
                    {
                        Origem = fields[0],
                        Destino = fields[1],
                        Valor = Convert.ToInt32(fields[2])
                    });
                }
            }
            _rotaService.AdicionarRotas(rotas);
            if (!OperationValid())
            {
                foreach (var msg in _notifier.GetNotifications().Select(n => n.Message))
                {
                    Console.WriteLine(msg);

                }
                return;
            }

            while (true)
            {
                Console.WriteLine("");
                Console.WriteLine("Aperte (Ctl + C) para sair ou digite a rota: ex (GRU-SCL)");
                var rotakey = Console.ReadLine();
                var resultadoFinal = _rotaService.MelhorRota(rotakey);
                if (!OperationValid())
                    Console.WriteLine(_notifier.GetNotifications().Select(n => n.Message).First().ToString());
                else
                    Console.WriteLine(resultadoFinal);

                _notifier.ClearErros();

            }
        }

        protected bool OperationValid()
        {
            if (_notifier.HasErroCode() || _notifier.HasNotification())
                return false;

            return true;
        }
    }
}
