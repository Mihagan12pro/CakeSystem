using CakeSystem.Domain.Entities;
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

            modelBuilder.Entity<Component>()
                .HasIndex(c => c.Type);
            modelBuilder.Entity<Component>()
                .Property(u => u.Count)
                .HasDefaultValue(0);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Manager)
                .WithMany()
                .HasForeignKey(o => o.ManagerLogin);
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Client)
                .WithMany()
                .HasForeignKey(o => o.ClientLogin);
            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>();
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Component> Components { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
