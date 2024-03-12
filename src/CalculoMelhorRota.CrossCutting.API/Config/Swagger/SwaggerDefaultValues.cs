using EConstrumarket.Construmanager.Core.CrossCutting.Util.Configs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EConstrumarket.Construmanager.Core.CrossCutting.API.Config.Swagger
{
    public class SwaggerDefaultValues : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiDescription = context.ApiDescription;

            operation.Deprecated = apiDescription.IsDeprecated();

            if (operation.Parameters == null)
            {
                return;
            }


            foreach (var parameters in operation.Parameters)
            {
                var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameters.Name);

                if (parameters.Description == null)
                {
                    parameters.Description = description.ModelMetadata?.Description;
                }

                parameters.Required |= description.IsRequired;
            }
        }
    }

    public class SwaggerAuthorizedMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly HttpClient _httpClient = new HttpClient();

        public SwaggerAuthorizedMiddleware(RequestDelegate next,  IOptions<AppSettingsUtils> settings)
        {
            //_httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.ActiveUrlAuth);

            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/swagger"))
            {
                string authHeader = context.Request.Headers["Authorization"];
                if (authHeader != null && authHeader.StartsWith("Basic "))
                {
                    // Get the credentials from request header
                    var header = AuthenticationHeaderValue.Parse(authHeader);
                    var inBytes = Convert.FromBase64String(header.Parameter);
                    var credentials = Encoding.UTF8.GetString(inBytes).Split(':');
                    var username = credentials[0];
                    var password = credentials[1];

                    var dado = new { Login= username ,Senha = password };

                    var data = new StringContent(JsonSerializer.Serialize(dado), Encoding.UTF8, "application/json");

                    var response = await _httpClient.PostAsync("api/auth/Login", data);

                    if (response.IsSuccessStatusCode)
                    {
                        await _next.Invoke(context).ConfigureAwait(false);
                        return;
                    }
 
                }
                context.Response.Headers["WWW-Authenticate"] = "Basic";
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                await _next.Invoke(context).ConfigureAwait(false);
            }
        }
    }
}
