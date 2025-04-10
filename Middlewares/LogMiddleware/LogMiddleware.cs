using System.Diagnostics;

namespace Middlewares.LogMiddleware
{
    public class LogMiddleware
    {
        private readonly RequestDelegate _next;

        public LogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var request = context.Request;
            var user = context.User?.FindFirst("userId")?.Value ?? "Anonymous";
            var method = request.Method;
            var path = request.Path;
            var controller = context.GetEndpoint()?.DisplayName ?? "Unknown";
            int statusCode = 200; // 🔧 התיקון כאן

            try
            {
                await _next(context);
                statusCode = context.Response.StatusCode;
            }
            catch (Exception ex)
            {
                statusCode = 500;
                await File.AppendAllTextAsync("logs.txt", $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] EXCEPTION: {ex.Message}\n");
                throw;
            }
            finally
            {
                stopwatch.Stop();
                var logLine = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {method} {path} | Controller: {controller} | User: {user} | Duration: {stopwatch.ElapsedMilliseconds}ms | Status: {statusCode}\n";
                await File.AppendAllTextAsync("logs.txt", logLine);
            }
        }
    }

    public static class OurLogMiddlewareHelper
    {
        public static void UseLogMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<LogMiddleware>();
        }
    }
}
