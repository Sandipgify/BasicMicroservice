using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Product.Application.Infrastructure;
using Product.Application.Interface;
using Product.Application.Provider;
using Product.Application.Provider.Interfaces;
using Product.Application.Services;

namespace Product.Application
{
    public static class DependencyResolution
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyResolution).Assembly;
            services.AddValidatorsFromAssembly(assembly);

            services.AddSingleton<IKafkaProducerProvider, KafkaProducerProvider>();

            #region services
            services.AddScoped<ICategoryService,CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            #endregion
            return services;
        }
    }
}
