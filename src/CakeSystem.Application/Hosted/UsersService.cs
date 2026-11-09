using CakeSystem.Application.Utils;
using Microsoft.Extensions.Hosting;

namespace CakeSystem.Application.Hosted
{
    public class UsersService : BackgroundService
    {
        private readonly RootPath _rootPath;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (File.Exists(Path.Combine(_rootPath.Path, "Static", "Users.xls")))
                {
                    
                }
            }
        }

        public UsersService(RootPath rootPath)
        {
            _rootPath = rootPath;
        }
    }
}
