using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});
app.Use(async (context, next) =>
{
    // فقط برای متدهایی که بدنه دارند چک کن
    if (context.Request.Method == HttpMethods.Post)
    {
        // اگر هدر وجود نداشت یا مقدارش غیر json بود
        if (!context.Request.ContentType?.StartsWith("application/json", StringComparison.OrdinalIgnoreCase) ?? true)
        {
            context.Response.StatusCode = StatusCodes.Status415UnsupportedMediaType;
            await context.Response.WriteAsync("Unsupported Media Type. Content-Type must be application/json.");
            return;
        }
    }

    if (!HttpMethods.IsPost(context.Request.Method))
    {
        context.Response.StatusCode = StatusCodes.Status405MethodNotAllowed;
        context.Response.ContentType = "application/json; charset=utf-8";

        await context.Response.WriteAsync("{\"error\": \"فقط درخواست POST مجاز است.\"}");
        return;
    }

    await next();
});
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();