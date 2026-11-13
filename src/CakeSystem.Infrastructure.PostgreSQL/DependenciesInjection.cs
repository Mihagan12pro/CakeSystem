using CakeSystem.Application.Services.Auth;
using CakeSystem.Application.Services.Components;
using CakeSystem.Infrastructure.PostgreSQL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CakeSystem.Infrastructure.PostgreSQL
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddDbServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IComponentsRepository, ComponentsRepository>();

            services.AddDbContext<AppDbContext>((options) =>
            {
                var configuration = new ConfigurationBuilder()
                    .AddUserSecrets<AppDbContext>()
                    .Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection");

                options.UseNpgsql(connectionString);
            });

            return services;
        }
    }
}
