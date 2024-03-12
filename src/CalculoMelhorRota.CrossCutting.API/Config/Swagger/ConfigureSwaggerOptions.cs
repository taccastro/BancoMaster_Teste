using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace EConstrumarket.Construmanager.Core.CrossCutting.API.Config.Swagger
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
            => this.provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var descripton in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(descripton.GroupName, CreateInfoForApiVersion(descripton));
            }
        }

        static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription descripton)
        {
            var info = new OpenApiInfo()
            {
                Title = "e-Construmanager -  Construmanager API",
                Version = "v1",
                Description = "Descrição de operação API CM",
                //TermsOfService = new Uri("https://example.com/terms"),
                Contact = new OpenApiContact
                {
                    Name = "Suporte Construmanager",
                    Email = "suporte@construmanager.com.br",
                    Url = new Uri("https://twitter.com/jwalkner"),
                },
                //License = new OpenApiLicense
                //{
                //    Name = "Employee API LICX",
                //    Url = new Uri("https://example.com/license"),
                //}
            };

            if (descripton.IsDeprecated)
            {
                info.Description += "Esta versão de API esta obsoleta";
            }
            return info;
        }
    }
}
