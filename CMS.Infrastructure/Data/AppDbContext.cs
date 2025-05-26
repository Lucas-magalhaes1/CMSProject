using CMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Template> Templates { get; set; }
    public DbSet<Conteudo> Conteudos { get; set; }  // DbSet para Conteúdo
    public DbSet<CampoPreenchido> CampoPreenchidos { get; set; }  // DbSet para CampoPreenchido

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>().ToTable("Usuarios");

        modelBuilder.Entity<Usuario>()
            .Property(u => u.Papel)
            .HasConversion<string>();

        modelBuilder.Entity<Template>().ToTable("Templates");

        // Configuração para Owned Entity (Value Object) de Template
        modelBuilder.Entity<Template>()
            .OwnsMany(t => t.Campos, cb =>
            {
                cb.WithOwner().HasForeignKey("TemplateId");
                cb.Property<Guid>("Id");
                cb.HasKey("Id");
                cb.ToTable("TemplateCampos");
            });

        // Relacionamento entre Conteudo e Template
        modelBuilder.Entity<Conteudo>()
            .HasOne(c => c.Template)
            .WithMany()
            .HasForeignKey("TemplateId");

        // Configuração da entidade CampoPreenchido
        modelBuilder.Entity<Conteudo>()
            .OwnsMany(c => c.CamposPreenchidos, cb =>
            {
                cb.WithOwner().HasForeignKey("ConteudoId");
                cb.Property<Guid>("Id");
                cb.HasKey("Id");
                cb.ToTable("CampoPreenchidos");
            });

        // Configuração do campo CriadoPor (autor do conteúdo)
        modelBuilder.Entity<Conteudo>()
            .Property(c => c.CriadoPor)  // O campo CriadoPor foi adicionado
            .IsRequired(); // Definir como obrigatório

        base.OnModelCreating(modelBuilder);
    }
}