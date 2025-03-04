namespace Middlewares.FirstMiddleware;
public class FirstMiddleware{

private RequestDelegate n;

    public FirstMiddleware(RequestDelegate next){
        this.n = next;
    }

    public async Task Invoke(HttpContext c){
        await c.Response.WriteAsync("first middleware start/n");
        await n(c);
         await c.Response.WriteAsync("first middleware end");
    }

}

public static partial class MiddlewaresExtensions
{
    public static WebApplication UseFirstMiddleware(this WebApplication app)
    {
        app.UseMiddleware<FirstMiddleware>();
        return app;
    }
}