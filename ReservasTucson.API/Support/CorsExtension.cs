using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text.Json;
// using ReservasTucson.API.Support.Handlers;
// using ReservasTucson.API.Support.Helpers;
//using ReservasTucson.Domain.Support.Helpers;

namespace ReservasTucson.API.Support
{
    public static class CorsExtension
    {
        /// <summary>
        /// Metodo extensivo para la configuracion de los services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddReservasTucsonCors(
            this IServiceCollection services)
        {
            services.AddCors(options =>
                options.AddPolicy(name: "AllowAccess_To_API",
                    policy => policy
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                ));

            return services;
        }
    }
}

