using CalculoMelhorRota.CrossCutting.Util.Configs;
using CalculoMelhorRota.Domain.Entity;
using CalculoMelhorRota.Domain.Service;
using CalculoMelhorRota.Infra.Data.Repositories;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CalculoMelhorRota.Domain.Tests
{
    public class RotaTests
    {
        private readonly IOptions<AppSettingsUtils> _appSettingsOptions = Options.Create(
            new AppSettingsUtils()
            {
                Path = "C:\\App_Rotas\\rotas.csv"
            });

        [Fact]
        public void Rota_Insert()
        {
            // arrange
            var mockNotifier = new Mock<INotifier>();
            var mockRotaRepository = new RotasRepository(_appSettingsOptions);

            // act 
            var rotaService = new RotasService(mockNotifier.Object, mockRotaRepository);
            var rotas = new List<Rotas>();

            rotas.Add(new Rotas { Origem = "GRU", Destino = "BRC", Valor = 10 });
            var result = rotaService.AdicionarRotas(rotas);

            // assert
            Assert.Equal(rotas.Count(), result.Count());

        }

        [Theory(DisplayName = "Executa Melhor Rota")]
        [InlineData("GRU", "BRC", "Melhor Rota: GRU - BRC ao custo de R$:10")]
        [InlineData("GRU", "CDG", "Melhor Rota: GRU - BRC - SCL - ORL - CDG ao custo de R$:40")]
        [InlineData("SCL", "GRU", "Melhor Rota: SCL - ORL - GRU ao custo de R$:45")]
        [InlineData("BRC", "SCL", "Melhor Rota: BRC - SCL ao custo de R$:5")]
        [InlineData("GRU", "CFD", "Melhor Rota: GRU - BRC - SCL - ORL - CDG - CFD ao custo de R$:70")]

        public void Melhor_Rota(string origem, string destino, string resultado)
        {
            // arrange
            var mockNotifier = new Mock<INotifier>();
            var mockRotaRepository = new RotasRepository(_appSettingsOptions);

            // act 
            var rotaService = new RotasService(mockNotifier.Object, mockRotaRepository);
            var resultadoExecucao = rotaService.MelhorRota($"{origem}-{destino}");

            // assert
            Assert.Equal(resultado, resultadoExecucao);

        }

        [Fact]
        public void Get_Rotas()
        {
            // arrange
            var mockNotifier = new Mock<INotifier>();
            var mockRotaRepository = new RotasRepository(_appSettingsOptions);

            // act 
            var rotaService = new RotasService(mockNotifier.Object, mockRotaRepository);
            var result = rotaService.GetRotas();

            // assert
            Assert.True(result.Any());
        }
    }
}
