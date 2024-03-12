using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CalculoMelhorRota.CrossCutting.IOC.AutoMapping
{
    public static class CustomAutoMapping
    {
        public static void AddCustomAutoMapping(this IServiceCollection services, IConfiguration configuration)
        {
            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
