using CakeSystem.Application.Hosted;
using CakeSystem.Application.Security.Hashers.SHA256;
using CakeSystem.Application.Services.Auth;
using CakeSystem.Application.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace CakeSystem.Application
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddServices();
            services.AddHostedServices();
            services.AddSecurity();
            services.AddUtils();

            return services;
        }

        private static IServiceCollection AddUtils(this IServiceCollection services)
        {
            services.AddSingleton<RootPath>();

            return services;
        }

        private static IServiceCollection AddHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<UsersBackgroundService>();
            services.AddHostedService<ComponentsBackgroundService>();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }

        private static IServiceCollection AddSecurity(this IServiceCollection services)
        {
            services.AddScoped<ISha256Hasher, Sha256Hasher>();

            return services;
        }
    }
}
