using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text.Json;
using ReservasTucson.Domain.Support.Helpers;

namespace ReservasTucson.API.Support.Handlers
{
    public class ExceptionMiddleware
    {
        #region Dependencias
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        #endregion

        #region Constructor
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        #endregion

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BackendException ex)
            {
                await HandleExceptionAsync(context, ex);
            }            
            catch (SecurityTokenExpiredException ex)
            {
                await HandleExceptionAsync(context, ex, StatusCodes.Status401Unauthorized, "El Token ha expirado");
            }
            catch (AccessViolationException ex)
            {
                await HandleExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        #region Helpers

        private void LogException(string title, string message)
        {
            _logger?.LogError($"{title} - {message}");
        }

        private Task HandleExceptionAsync(
            HttpContext context,
            Exception exception,
            int? overrideStatusCode = null,
            string? overrideTitle = null)
        {
            int code = overrideStatusCode ?? (exception is BackendException be ? be.StatusCode : StatusCodes.Status500InternalServerError);
            string title = overrideTitle ?? (exception is BackendException be2 ? be2.Title : "Server Error");

            List<string> errors = new List<string>();

            if (exception is BackendException be3)
            {
                errors.AddRange(be3.Errors);
            }
            else
            {
                errors.Add(exception.InnerException?.Message ?? exception.Message);
            }

            // Logging
            LogException(title, string.Join(", ", errors));

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = code;

            var response = new BackendError()
            {
                StatusCode = code,
                Title = title,
                Errors = errors
            };

            return context.Response.WriteAsync(response.ToJson());
        }

        #endregion
    }
}

