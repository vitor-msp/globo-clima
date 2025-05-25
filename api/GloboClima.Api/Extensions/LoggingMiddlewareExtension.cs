using GloboClima.Api.Middlewares;

namespace GloboClima.Api.Extensions;

public static class LoggingMiddlewareExtension
{
    public static IApplicationBuilder UseLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LoggingMiddleware>();
    }
}