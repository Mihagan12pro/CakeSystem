using CakeSystem.Application.DTOs.Auth;
using CakeSystem.Application.Security.Hashers.SHA256;
using CakeSystem.Application.Services.Auth;
using CakeSystem.Application.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Linq.Expressions;

namespace CakeSystem.Application.Hosted
{
    public class UsersService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly RootPath _rootPath;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                string path = Path.Combine(_rootPath.Path, "Static", "Users.xls");

                if (File.Exists(path))
                {
                    using(var scope = _serviceScopeFactory.CreateScope())
                    {
                        IServiceProvider serviceProvider = scope.ServiceProvider;

                        ISha256Hasher hasher = serviceProvider.GetRequiredService<ISha256Hasher>();
                        IAuthService authService = serviceProvider.GetRequiredService<IAuthService>();

                        IWorkbook workbook;
                        try
                        {
                            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
                            {
                                workbook = new HSSFWorkbook(fileStream);
                                ISheet sheet = workbook.GetSheetAt(0);

                                IRow cells = sheet.GetRow(1);

                                string login = cells.GetCell(2).StringCellValue;
                                string password = hasher.Hash(cells.GetCell(3).StringCellValue);
                                string role = StringCleaner.CleanCyrillic(cells.GetCell(4).StringCellValue);
                                string fullName = cells.GetCell(5).StringCellValue;

                                PutUserDto user = new PutUserDto(login, password, role, fullName);
                                await authService.PutOrAddAsync(user, stoppingToken);
                            }
                        }
                        catch (IOException ex)
                        {

                        }
                    }
                }
            }
        }

        public UsersService(
            IServiceScopeFactory serviceScopeFactory,
            RootPath rootPath)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _rootPath = rootPath;
        }
    }
}
