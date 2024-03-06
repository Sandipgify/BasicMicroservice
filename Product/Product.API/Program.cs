using Product.API;
using Product.Application;
using Product.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices().
    AddApplication().
    AddInfrastructure();

builder.Services.AddServices();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSwaggerGen();
});


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

app.UseAuthorization();

app.MapControllers();

app.Run();
