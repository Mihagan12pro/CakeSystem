using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace CakeSystem.Infrastructure.SQLite
{
    public class AppDbContext : DbContext
    {
        private readonly IHostEnvironment _env;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.Combine(new DirectoryInfo(_env.ContentRootPath).Parent.Parent.FullName, "database.db");

            optionsBuilder.UseSqlite($"Data Source={dbPath}");
            SQLitePCL.Batteries.Init();
        }

        public AppDbContext(IHostEnvironment env)
        {
            _env = env;
        }
    }
}
