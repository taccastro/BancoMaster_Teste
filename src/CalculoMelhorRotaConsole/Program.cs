using CalculoMelhorRota.CrossCutting.IOC.DependencyInjection;
using CalculoMelhorRota.CrossCutting.Util.Configs;
using CalculoMelhorRota.Domain.Interfaces;
using CalculoMelhorRotaConsole.Config;
using CalculoMelhorRotaConsole.Interfaces;
using CalculoMelhorRotaConsole.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CalculoMelhorRotaConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                string arg = args.Any() ? args[0] : "";

#if DEBUG
                arg = $@"{Directory.GetCurrentDirectory()}\rotas.csv";
#endif

                if (string.IsNullOrEmpty(arg) || Path.GetExtension(arg).ToLower() != ".csv")
                {
                    string name = Assembly.GetExecutingAssembly().ManifestModule.ToString().Replace(".dll", ".exe");
                    Console.WriteLine();
                    Console.WriteLine(@$"Aplicativo console deve ser inicializado com arquivo CSV nos parametros ex:");
                    Console.WriteLine(@$"{name} ""C:\Folder\FILE.csv""");
                    Console.WriteLine();

                    return;
                }
                //Execução App

                #region Injeção de dependencia
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                    .AddJsonFile("appsettings.json", optional: true);

                var Configuration = builder.Build();

                var serviceCollection = new ServiceCollection();
                serviceCollection.AddCustomService();
                serviceCollection.AddCustomServiceApp();

                serviceCollection.Configure<AppSettingsUtils>(Configuration.GetSection(nameof(AppSettingsUtils)));

                var serviceProvider = serviceCollection.BuildServiceProvider();
                var appService = serviceProvider.GetService<IAppService>();
                #endregion        

                appService.ExecutaCalculoRota(arg);

            }
            catch (Exception ex)
            {

                throw;
            }

        }

    }
}
