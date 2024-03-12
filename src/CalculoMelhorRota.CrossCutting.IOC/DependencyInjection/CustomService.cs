using CalculoMelhorRota.Application.AppServices;
using CalculoMelhorRota.Application.Interfaces.AppServices;
using CalculoMelhorRota.CrossCutting.Util.Configs;
using CalculoMelhorRota.Domain.Interfaces;
using CalculoMelhorRota.Domain.Service;
using CalculoMelhorRota.Infra.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CalculoMelhorRota.CrossCutting.IOC.DependencyInjection
{

    public static class CustomService
    {

        public static void AddCustomService(this IServiceCollection services)
        {
            #region Injeções Serviços
            services.AddScoped<IRotasAppService, RotasAppService>()
                    .AddScoped<IRotasService, RotasService>()
                    .AddScoped<IGlobalAppService, GlobalAppService>()
                    .AddScoped<INotifier, Notifier>()
;
            #endregion

            #region Injeções Repositórios

            services.AddScoped<IRotasRepository, RotasRepository>();

            #endregion

        }
    }
}
