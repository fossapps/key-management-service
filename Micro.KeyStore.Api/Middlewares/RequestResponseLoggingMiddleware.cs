using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Micro.KeyStore.Api.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);
            if (context.Response.StatusCode != StatusCodes.Status404NotFound)
            {
                return;
            }
            var builder = new StringBuilder();
            foreach (var (key, value) in context.Request.Headers)
            {
                builder.AppendLine($"{key}:{value}");
            }
            _logger.LogDebug($"404: {context.Request.Path} -> \n{builder}");
        }
    }
}
