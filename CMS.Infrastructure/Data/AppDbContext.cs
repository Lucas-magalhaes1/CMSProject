using CMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using CMS.Domain.Enums;

namespace CMS.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Papel)
                .HasConversion<string>();

            base.OnModelCreating(modelBuilder);
        }
    }
}