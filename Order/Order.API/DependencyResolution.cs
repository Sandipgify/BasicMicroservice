using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Order.Infrastructure.Context;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;

namespace Order.API;
public static class DependencyResolution
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }


    public static SwaggerGenOptions AddSwaggerGen(this SwaggerGenOptions options)
    {
        options.SwaggerDoc("order", new OpenApiInfo { Title = "Order" });

        options.CustomSchemaIds(type => type.FullName);

        return options;
    }

    public static SwaggerUIOptions UseSwaggerEndpoint(this SwaggerUIOptions uiOptions)
    {
        uiOptions.SwaggerEndpoint("/swagger/order/swagger.json", "Order");
        return uiOptions;
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEntityFrameworkNpgsql().AddDbContext<OrderDbContext>(opt =>
        {
            opt.UseNpgsql(configuration.GetConnectionString("ConnectionStr"), x => x.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName));
        });

        return services;
    }

}
