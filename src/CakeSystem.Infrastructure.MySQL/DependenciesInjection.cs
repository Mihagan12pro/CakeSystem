using CakeSystem.Application.Services.Auth;
using CakeSystem.Infrastructure.MySQL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CakeSystem.Infrastructure.MySQL
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddDbServices(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<IAuthRepository, AuthRepository>(
                (options) => new AuthRepository(connectionString)
            );

            return services;
        }
    }
}
