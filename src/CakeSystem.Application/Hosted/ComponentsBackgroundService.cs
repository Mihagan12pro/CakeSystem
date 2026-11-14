using CakeSystem.Application.Extensions;
using CakeSystem.Application.Security.Hashers.SHA256;
using CakeSystem.Application.Services.Components;
using CakeSystem.Application.Utils;
using CakeSystem.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NPOI.HSSF.UserModel;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;
using System.ComponentModel;
using Component = CakeSystem.Domain.Component;

namespace CakeSystem.Application.Hosted
{
    public class ComponentsBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<ComponentsBackgroundService> _logger;
        private readonly RootPath _rootPath;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(3000);

                string path = Path.Combine(_rootPath.Path, "Static", "Components.xls");

                if (File.Exists(path))
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        try
                        {
                            IServiceProvider serviceProvider = scope.ServiceProvider;

                            ISha256Hasher hasher = serviceProvider.GetRequiredService<ISha256Hasher>();
                            IComponentsRepository repository = serviceProvider.GetRequiredService<IComponentsRepository>();

                            IWorkbook workbook;

                            using (FileStream input = new FileStream(path, FileMode.Open, FileAccess.Read))
                            {
                                workbook = new HSSFWorkbook(input);
                            }

                            ISheet sheet = workbook.GetSheetAt(0);

                            LinkedList<Component> components = new LinkedList<Component>();
                            

                            foreach (var row in sheet.Rows)
                            {
                                if (row.Cells.Count > 0 && row.RowNum > 0)
                                {
                                    try
                                    {
                                        Component component = new Component
                                        {
                                            Id = Convert.ToInt32(row.GetCell(0).GetObjectCellValue()),

                                            Title = row.GetCell(1).GetObjectCellValue().ToString(),

                                            UnitMeasurement = row.GetCell(2).GetObjectCellValue().ToString(),

                                            Type = row.GetCell(4).GetObjectCellValue().ToString(),

                                            Price = Convert.ToDecimal(row.GetCell(5).GetObjectCellValue()),

                                            PrePacking = row.GetCell(7).GetObjectCellValue().ToString()
                                        };

                                        if (row.GetCell(3).CellType != CellType.Blank)
                                            component.Count = Convert.ToInt32(row.GetCell(3).GetObjectCellValue());

                                        if (row.GetCell(6).CellType != CellType.Blank)
                                            component.Gost = row.GetCell(6).GetObjectCellValue().ToString();

                                        if (row.GetCell(8).CellType != CellType.Blank)
                                            component.Characteristics = row.GetCell(8).GetObjectCellValue().ToString();

                                        components.AddLast(component);
                                    }
                                    catch (NotOLE2FileException ex)
                                    {
                                        _logger.LogError(ex, "Invalid xls file!");
                                    }
                                    catch (IOException ex)
                                    {
                                        _logger.LogWarning(ex, "Conflict!");
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogWarning(ex, "The {0} row caused exception:", row.RowNum + 1);
                                    }
                                }
                            }

                            await repository.AddAsync(components, stoppingToken);

                        }
                        catch(NotOLE2FileException ex)
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

        public ComponentsBackgroundService(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<ComponentsBackgroundService> logger,
            RootPath rootPath)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _rootPath = rootPath;
        }
    }
}
