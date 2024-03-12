using CalculoMelhorRotaConsole.Interfaces;
using CalculoMelhorRotaConsole.Service;
using Microsoft.Extensions.DependencyInjection;

namespace CalculoMelhorRotaConsole.Config
{
    public static class CustomServiceApp
    {
        public static void AddCustomServiceApp(this IServiceCollection services)
        {
            #region Injeções Serviços
            services.AddScoped<IAppService, AppService>();
            #endregion
        }
    }
}
