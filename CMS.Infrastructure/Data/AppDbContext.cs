using CMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Template> Templates { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>().ToTable("Usuarios");

        modelBuilder.Entity<Usuario>()
            .Property(u => u.Papel)
            .HasConversion<string>();

        modelBuilder.Entity<Template>().ToTable("Templates");

        // Configuração para Owned Entity (Value Object)
        modelBuilder.Entity<Template>()
            .OwnsMany(t => t.Campos, cb =>
            {
                cb.WithOwner().HasForeignKey("TemplateId");
                cb.Property<Guid>("Id");
                cb.HasKey("Id");
                cb.ToTable("TemplateCampos"); // tabela separada mas ligada à Template
            });

        base.OnModelCreating(modelBuilder);
    }
}