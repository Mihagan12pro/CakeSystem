using CakeSystem.Application.DTOs.Auth;
using CakeSystem.Application.Security.Hashers.SHA256;
using CakeSystem.Application.Services.Auth;
using CakeSystem.Application.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NPOI.HSSF.UserModel;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;

namespace CakeSystem.Application.Hosted
{
    public class UsersBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<UsersBackgroundService> _logger;
        private readonly RootPath _rootPath;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int delay = 3000;

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(delay);

                string path = Path.Combine(_rootPath.Path, "Static", "Users.xls");

                if (File.Exists(path))
                {
                    using(var scope = _serviceScopeFactory.CreateScope())
                    {
                        try
                        {
                            IServiceProvider serviceProvider = scope.ServiceProvider;

                            ISha256Hasher hasher = serviceProvider.GetRequiredService<ISha256Hasher>();
                            IAuthService authService = serviceProvider.GetRequiredService<IAuthService>();

                            IWorkbook workbook;
 
                            using (FileStream input = new FileStream(path, FileMode.Open, FileAccess.Read))
                            {
                                workbook = new HSSFWorkbook(input);
                            }

                            ISheet sheet = workbook.GetSheetAt(0);

                            foreach(var row in sheet.Rows)
                            {
                                if (row.Cells.Count > 0)
                                {
                                    delay = 3000;
                                    try
                                    {
                                        string login = row.GetCell(0).StringCellValue;
                                        string password = hasher.Hash(row.GetCell(1).StringCellValue);
                                        string role = StringCleaner.CleanCyrillic(row.GetCell(2).StringCellValue);
                                        string fullName = row.GetCell(3).StringCellValue;
                                        PutUserDto user = new PutUserDto(login, password, role, fullName);
                                        await authService.PutOrAddAsync(user, stoppingToken);

                                        sheet.RemoveRow(row);
                                        sheet.ShiftRows(row.RowNum, sheet.LastRowNum, -1);
                                    }
                                    catch (NotOLE2FileException ex)
                                    {
                                        _logger.LogError(ex, "Invalid xls file!");
                                    }
                                    catch (IOException ex)
                                    {
                                        _logger.LogWarning(ex, "Conflict!");
                                    }
                                    catch(Exception ex)
                                    {
                                        _logger.LogWarning(ex, "The {0} row caused exception:", row.RowNum + 1);
                                    }
                                }
                                else
                                {
                                    delay = 10000;
                                }
                            }

                            using (FileStream output = new FileStream(path, FileMode.Create, FileAccess.Write))
                            {
                                workbook.Write(output);
                            }

                            workbook.Close();

                        }
                        catch (NotOLE2FileException ex)
                        {
                            _logger.LogError(ex, "Invalid xls file!");
                        }
                        catch (IOException ex)
                        {
                            _logger.LogWarning(ex, "Conflict!");
                        }
                    }
                }
            }
        }

        public UsersBackgroundService(
            ILogger<UsersBackgroundService> logger,
            IServiceScopeFactory serviceScopeFactory,
            RootPath rootPath)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _rootPath = rootPath;
        }
    }
}
