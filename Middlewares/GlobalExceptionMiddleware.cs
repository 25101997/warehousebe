using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using Npgsql;

namespace WarehouseAPI.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Continúa el pipeline
                await _next(context);
            }
            catch (Exception ex)
            {
                // Atrapas cualquier excepción que no haya sido manejada
                _logger.LogError(ex, "Error no manejado capturado en el middleware global");

                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // Configurar el tipo de contenido
            context.Response.ContentType = "application/json";

            // Por defecto, consideramos que es un error interno (500)
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var message = "Ocurrió un error inesperado en el servidor.";

            // Si detectas una excepción de PostgreSQL (NpgsqlException), podrías retornar 503 (Service Unavailable)
            // o el que tú consideres apropiado:
            if (ex is NpgsqlException)
            {
                statusCode = (int)HttpStatusCode.ServiceUnavailable;
                message = "La base de datos no está disponible en este momento.";
            }
            // Podrías añadir más condiciones si quieres manejar otras excepciones específicas.

            context.Response.StatusCode = statusCode;

            // Retornar un JSON con la información necesaria
            var result = new 
            {
                StatusCode = statusCode,
                Error = message,
                Detail = ex.Message // Opcional: puedes omitir el detalle para no exponer info sensible
            };

            return context.Response.WriteAsJsonAsync(result);
        }
    }
}
