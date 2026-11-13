using CakeSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace CakeSystem.Infrastructure.PostgreSQL
{
    public class AppDbContext : DbContext
    {
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
