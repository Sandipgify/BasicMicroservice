using Auth.API;
using Auth.Data;
using Auth.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddData().AddApplication();

builder.Services.AddServices();

builder.Services.AddDbContext(builder.Configuration);

builder.Services.AddSwaggerGen(options =>
{
    options.AddSwaggerGen();
});

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

app.MapControllers();

app.Run();
