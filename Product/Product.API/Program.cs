using Product.API;
using Product.API.Middleware;
using Product.Application;
using Product.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices().
    AddApplication().
    AddInfrastructure();

builder.Services.AddServices();

builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddTransient<GlobalErrorHandler>();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSwaggerGen();
    Product.API.DependencyResolution.ConfigureAuthentication(options);
});


Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Services.AddAuthentication(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.UseSwaggerEndpoint();
    });

}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<GlobalErrorHandler>();

app.MapControllers();

app.Run();
