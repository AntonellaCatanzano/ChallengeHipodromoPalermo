using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace ReservasTucson.API.Support
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddCustomizedSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "ReservasTucson - API",
                        Description = "Sistema de Reservas de Eventos",
                        Version = "v1",
                    });

                // ---------- CONFIGURACIÓN JWT COMPLETA ----------
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Ingrese: Bearer {su_token_jwt}",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });

                // ---------- XML Documentation ----------
                try
                {
                    string docName = "ServiceDocumentation";
                    var xmlFile = $"{docName}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                    if (File.Exists(xmlPath))
                        options.IncludeXmlComments(xmlPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Swagger XML Error: {ex.Message}");
                }
            });

            return services;
        }

        public static IApplicationBuilder UseReservasTucsonSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.DocumentTitle = "ReservasTucson Swagger UI";
                options.SwaggerEndpoint("../swagger/v1/swagger.json", "ReservasTucson.API v1");


                options.InjectStylesheet("../css/style.css");
                options.InjectJavascript("../js/swagger.js", "text/javascript");
            });

            return app;
        }
    }
}

