using Auth.Service.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Service
{
    public static class DependencyResolution
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyResolution).Assembly;
            services.AddValidatorsFromAssembly(assembly);

            #region services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            #endregion
            return services;
        }
    }
}
