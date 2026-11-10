using Microsoft.Extensions.DependencyInjection;

namespace CakeSystem.Infrastructure.MySQL
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddDbServices(this IServiceCollection services, string connectionString)
        {


            return services;
        }
    }
}
