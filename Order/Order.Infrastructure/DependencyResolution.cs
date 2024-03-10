using Microsoft.Extensions.DependencyInjection;
using Order.Infrastructure.Repository;

namespace Order.Infrastructure
{
    public static class DependencyResolution
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            return services;
        }
    }
}
