using Microsoft.Extensions.Caching.Memory;
using Microsoft.Net.Http.Headers;
using System.Globalization;
using System.Net.Http.Headers;
using System.Net;
using System.Text.Json;

namespace TaskManagerAPI.Extensions
{
    /// <summary>
    /// Middleware para tratamento global de exceções.
    /// Captura exceções não tratadas em toda a aplicação e retorna 
    /// uma resposta JSON padronizada com detalhes da exceção.
    /// </summary>
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;            
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {                
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;

            var exceptionMessage = exception.Message;
            var innerExceptionMessage = exception.InnerException?.Message;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            var jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web
                .JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

            var result = JsonSerializer.Serialize(new { Exception = exceptionMessage, Details = innerExceptionMessage }, jso);

            return context.Response.WriteAsync(result);
        }
    }

    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
            => builder.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}
