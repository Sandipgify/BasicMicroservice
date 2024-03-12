using Auth.Data.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Data
{
    public static class DependencyResolution
    {
        public static IServiceCollection AddData(this IServiceCollection services)
        {            
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}
