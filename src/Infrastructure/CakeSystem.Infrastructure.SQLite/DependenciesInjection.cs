using CakeSystem.Application.Services.Auth;
using CakeSystem.Infrastructure.SQLite.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CakeSystem.Infrastructure.SQLite
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddDbServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthRepository, AuthRepository>();

            services.AddDbContext<AppDbContext>();

            return services;
        }
    }
}
