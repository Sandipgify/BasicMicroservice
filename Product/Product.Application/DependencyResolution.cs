using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Product.Application
{
    public static class DependencyResolution
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyResolution).Assembly;
            services.AddValidatorsFromAssembly(assembly);
            return services;
        }
    }
}
