namespace Middlewares.ErrorMiddleware;

public class ErrorMiddleware{

private RequestDelegate n;

    public ErrorMiddleware(RequestDelegate next){
        this.n = next;
    }

    public async Task Invoke(HttpContext c){
        c.Items["success"] = false;
       try
       {
         await n(c);
            c.Items["success"] = true;
       }
       catch (ApplicationException ex)
        {
            c.Response.StatusCode = 400;
            await c.Response.WriteAsync(ex.Message);
        }
        catch (Exception e)
        {
            c.Response.StatusCode = 500;
            await c.Response.WriteAsync($"פנה לתמיכה הטכנית /n{e.Message}");
        }
    }

}

public static partial class MiddlewaresExtensions
{
    public static WebApplication UseErrorMiddleware(this WebApplication app)
    {
        app.UseMiddleware<ErrorMiddleware>();
        return app;
    }
}