using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Order.Application.Interface;
using Order.Application.Services;

namespace Order.Application
{
    public static class DependencyResolution
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyResolution).Assembly;
            services.AddValidatorsFromAssembly(assembly);

            #region services
            services.AddScoped<IOrderService, OrderService>();
            #endregion
            return services;
        }
    }
}
