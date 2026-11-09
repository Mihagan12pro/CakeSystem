using CakeSystem.Application.Auth;
using CakeSystem.Infrastructure.SQLite.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CakeSystem.Infrastructure.SQLite
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddDbServices(IServiceCollection services)
        {
            services.AddScoped<IAuthRepository, AuthRepository>();

            services.AddDbContext<AppDbContext>();

            return services;
        }
    }
}
