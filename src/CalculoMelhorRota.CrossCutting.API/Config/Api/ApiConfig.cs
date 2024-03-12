using EConstrumarket.Construmanager.Core.CrossCutting.API.Config.Swagger;
using System.Collections.Generic;

namespace EConstrumarket.Construmanager.Core.CrossCutting.API.Config.Api
{
    public static class ApiConfig
    {
        private const string SpecifOrigensCors = "_specifOrigensCors";

        public static void WebApiConfig(this IServiceCollection services, IConfiguration configuration)
        {  

            #region Versão Api

            services.AddApiVersioning(opt =>
            {
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(opt =>
            {
                opt.GroupNameFormat = "'v'VVV";
                opt.SubstituteApiVersionInUrl = true;
            });

            #endregion        

            #region Swagger

            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<SwaggerDefaultValues>();

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Insira seu token JWT desta maneira: Bearer {seu token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer", //The name of the previously defined security scheme.
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });
            });
            #endregion

            services .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();


        }

        public static void Configure(this IApplicationBuilder app, IWebHostEnvironment env,
            IApiVersionDescriptionProvider provider)
        {      

            #region Sweeger
            //app.UseMiddleware<SwaggerAuthorizedMiddleware>();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(opt =>
            {
                opt.DocumentTitle = "CM - Help";
                
                opt.RoutePrefix = "help";
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    if (env.EnvironmentName == "Docker")
                    {
                        opt.SwaggerEndpoint($"../swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpper());
                        continue;
                    }
                    opt.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpper());
                }

            });
            #endregion
        }
    }
}
