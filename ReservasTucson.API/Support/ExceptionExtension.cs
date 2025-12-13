using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using ReservasTucson.API.Support.Handlers;
using ReservasTucson.API.Support.Helpers;
using ReservasTucson.Domain.Support.Helpers;
using System.Net;
using System.Text.Json;


namespace ReservasTucson.API.Support
{
    public static class ExceptionExtension
    {
        /// <summary>
        /// Configura el manejo de excepciones para el ModelState
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSMSExceptionHandler(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(o =>
            {
                o.InvalidModelStateResponseFactory = actionContext =>
                {
                    var logger = actionContext
                              .HttpContext
                              .RequestServices
                              .GetService<ILogger>();

                    List<string> errors = actionContext.ModelState.GetModelValidations();

                    string route = actionContext.ActionDescriptor.AttributeRouteInfo?.Template ?? "UnknownRoute";
                    string jsonModelState = JsonSerializer.Serialize(errors);
                    if (logger != null)
                    {
                        logger.LogWarning($"Endpoint [{route}] - {jsonModelState}");
                    }

                    return new BadRequestObjectResult(new BackendError()
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Title = "ModelState",
                        Errors = errors
                    });
                };
            });

            return services;
        }

        public static IApplicationBuilder UseEvaliaExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseExceptionHandler(appError =>
                appError.Run(async context =>
                {
                    var exception = context.Features.Get<IExceptionHandlerFeature>();
                    if (exception != null)
                    {
                        string errorTitle = "Global";

                        var code = context.Response?.StatusCode ?? (int)HttpStatusCode.InternalServerError;
                        var route = context.Request.Path;

                        var logger = context
                            .RequestServices
                            .GetService<ILogger>();

                        List<string> errors = new List<string>();

                        if (exception.Error is SecurityTokenExpiredException)
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                            code = (int)HttpStatusCode.Unauthorized;
                            errorTitle = "Authorization";
                        }
                        else if (exception.Error is BackendException apiException)
                        {
                            code = apiException.StatusCode;
                            errorTitle = apiException.Title;
                            errors.AddRange(apiException.Errors);
                        }
                        else if (exception.Error is SqlException sqlException)
                        {
                            code = 500;
                            errorTitle = "Database";

                            for (int i = 0; i < sqlException.Errors.Count; i++)
                            {
                                errors.Add(sqlException.Errors[i].Message);
                            }
                        }
                        else
                        {
                            string errorMessage = exception.Error.InnerException?.Message ?? exception.Error.Message;
                            errors.Add(errorMessage);
                        }

                        string jsonModelState = JsonSerializer.Serialize(errors);

                        if (logger != null)
                        {
                            logger.LogError($"Endpoint [{route}] - {jsonModelState}");
                        }

                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = code;

                        await context.Response.WriteAsync(new BackendError()
                        {
                            StatusCode = code,
                            Title = errorTitle,
                            Errors = errors
                        }.ToJson());
                    }
                })
            );

            return app;
        }
    }
}

