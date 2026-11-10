using CakeSystem.Application.Services.Auth;
using CakeSystem.Infrastructure.SQLite.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CakeSystem.Infrastructure.SQLite
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddDbServices(this IServiceCollection services, IHostEnvironment env)
        {
            services.AddScoped<IAuthRepository, AuthRepository>();

            services.AddDbContext<AppDbContext>((options => 
            {
                options.UseSqlite(Path.Combine(new DirectoryInfo(env.ContentRootPath).Parent.Parent.Parent.FullName, "database.db"));
            }));

            return services;
        }
    }
}
