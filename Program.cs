using Lesson3.Services;
using Lesson3.Interfaces;
using Microsoft.OpenApi.Models;
using Middlewares.FirstMiddleware;
using Middlewares.LogMiddleware;
using Middlewares.ErrorMiddleware;

var builder = WebApplication.CreateBuilder(args);

// הוספת שירותי ה-Controllers
builder.Services.AddControllers();
builder.Services.AddBookConst();

// הוספת Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

var app = builder.Build();

// קביעת המידלוואר לשרת את Swagger כנקודת JSON
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // קבעי את Swagger UI בשורש האפליקציה
    });
}

app.UseFirstMiddleware();
app.UseLogMiddleware();
app.UseErrorMiddleware();


app.Map("/test1", (a) => 
    a.Run(async context => 
    await context.Response.WriteAsync("our test1-map terminal middleware!\n")));

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();




