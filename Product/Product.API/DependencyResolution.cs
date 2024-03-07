using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Product.Infrastructure.Context;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;

namespace Product.API;
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
        options.SwaggerDoc("product", new OpenApiInfo { Title = "Product" });

        options.CustomSchemaIds(type => type.FullName);

        return options;
    }

    public static SwaggerUIOptions UseSwaggerEndpoint(this SwaggerUIOptions uiOptions)
    {
        uiOptions.SwaggerEndpoint("/swagger/product/swagger.json", "Product");
        return uiOptions;
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEntityFrameworkNpgsql().AddDbContext<ProductDbContext>(opt =>
        {
            opt.UseNpgsql(configuration.GetConnectionString("ConnectionStr"), x => x.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName));
        });

        return services;
    }

}
