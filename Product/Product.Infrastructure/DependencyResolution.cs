using Microsoft.Extensions.DependencyInjection;

namespace Product.Infrastructure
{
    public static class DependencyResolution
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            return services;
        }
    }
}
