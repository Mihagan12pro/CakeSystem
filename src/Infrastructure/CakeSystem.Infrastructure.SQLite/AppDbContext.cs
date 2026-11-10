using CakeSystem.Application.Utils;
using CakeSystem.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SkiaSharp;

namespace CakeSystem.Infrastructure.SQLite
{
    public class AppDbContext : DbContext
    {
        private readonly RootPath _rootPath;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => SQLitePCL.Batteries.Init();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>(); 
        }

        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}
