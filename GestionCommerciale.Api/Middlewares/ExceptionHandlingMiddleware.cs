using System.Net;
using System.Text.Json;

namespace GestionCommerciale.Api.Middlewares
{
    // Intercepte TOUTES les exceptions non gérées de l'application
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // laisse passer la requête vers le reste de l'app
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur non gérée interceptée par le middleware.");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = new { message = "Une erreur interne est survenue. Veuillez réessayer plus tard." };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}