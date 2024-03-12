using Auth.Data.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;
using System.Text;

namespace Auth.API
{
    public static class DependencyExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }
        public static SwaggerGenOptions AddSwaggerGen(this SwaggerGenOptions options)
        {
            options.SwaggerDoc("user", new OpenApiInfo { Title = "User" });

            options.CustomSchemaIds(type => type.FullName);

            return options;
        }

        public static SwaggerUIOptions UseSwaggerEndpoint(this SwaggerUIOptions uiOptions)
        {
            uiOptions.SwaggerEndpoint("/swagger/user/swagger.json", "User");
            return uiOptions;
        }

        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEntityFrameworkNpgsql().AddDbContext<AuthContext>(opt =>
            {
                opt.UseNpgsql(configuration.GetConnectionString("ConnectionStr"), x => x.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName));
            });

            return services;
        }

        

        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)

        {

           services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true
                };
            });
            return services;
        }
    }
}
