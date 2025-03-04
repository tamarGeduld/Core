using System.Diagnostics;

namespace Middlewares.LogMiddleware;
public class LogMiddleware{

private RequestDelegate n;

    public LogMiddleware(RequestDelegate next){
        this.n = next;
    }

    public async Task Invoke(HttpContext c){
        await c.Response.WriteAsync($"Our Log Middleware start\n");
         var sw = new Stopwatch();
        sw.Start();
        await n(c);
        Console.WriteLine($"{c.Request.Path}.{c.Request.Method} took {sw.ElapsedMilliseconds}ms."
            + $" Success: {c.Items["success"]}"
            + $" User: {c.User?.FindFirst("userId")?.Value ?? "unknown"}");
        await c.Response.WriteAsync("Our Log Middleware end\n");
    }



}

public static class OurLogMiddlewareHelper
{
    public static void UseLogMiddleware(this IApplicationBuilder a)
    {
        a.UseMiddleware<LogMiddleware>();
    }
}