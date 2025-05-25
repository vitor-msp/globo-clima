namespace GloboClima.Api.Middlewares;

public class LoggingMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        if (RequestIsHealthcheck(context))
        {
            await _next(context);
            return;
        }
        var start = DateTime.UtcNow;
        await _next(context);
        var duration = DateTime.UtcNow - start;
        LogRequest(context, duration);
    }

    private static void LogRequest(HttpContext context, TimeSpan duration)
    {
        var request = context.Request;
        var response = context.Response;
        var log = $"{context.Connection.RemoteIpAddress} " +
            $"- - " +
            $"[{DateTime.Now:dd/MMM/yyyy:HH:mm:ss zzz}] " +
            $"\"{request.Method} {request.Path}{request.QueryString} {request.Protocol}\" " +
            $"{response.StatusCode} " +
            $"{response.ContentLength ?? 0} " +
            $"\"{request.Headers["Referer"]}\" " +
            $"\"{request.Headers["User-Agent"]}\" " +
            $"{duration.TotalMilliseconds}ms";
        Console.WriteLine(log);
    }

    private static bool RequestIsHealthcheck(HttpContext context)
        => context.Request.Path.Equals("/healthcheck");
}