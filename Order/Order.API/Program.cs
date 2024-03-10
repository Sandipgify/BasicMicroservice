using Order.API.Middleware;


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
});

var app = builder.Build();

// Configure the HTTP request pipeline.
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

app.UseMiddleware<GlobalErrorHandler>();

app.MapControllers();

app.Run();
