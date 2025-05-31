using CMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Template> Templates { get; set; }
    public DbSet<Conteudo> Conteudos { get; set; }  
    public DbSet<CampoPreenchido> CampoPreenchidos { get; set; }  
    public DbSet<Notificacao> Notificacoes { get; set; } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>().ToTable("Usuarios");

        modelBuilder.Entity<Usuario>()
            .Property(u => u.Papel)
            .HasConversion<string>();

        modelBuilder.Entity<Template>().ToTable("Templates");
        
        modelBuilder.Entity<Template>()
            .OwnsMany(t => t.Campos, cb =>
            {
                cb.WithOwner().HasForeignKey("TemplateId");
                cb.Property<Guid>("Id");
                cb.HasKey("Id");
                cb.ToTable("TemplateCampos");
            });

       
        modelBuilder.Entity<Conteudo>()
            .HasOne(c => c.Template)
            .WithMany()
            .HasForeignKey("TemplateId");

       
        modelBuilder.Entity<Conteudo>()
            .OwnsMany(c => c.CamposPreenchidos, cb =>
            {
                cb.WithOwner().HasForeignKey("ConteudoId");
                cb.Property<Guid>("Id");
                cb.HasKey("Id");
                cb.ToTable("CampoPreenchidos");
            });

        
        modelBuilder.Entity<Conteudo>()
            .Property(c => c.CriadoPor)  
            .IsRequired(); 

        
        modelBuilder.Entity<Notificacao>(entity =>
        {
            entity.ToTable("Notificacoes");

            entity.HasKey(n => n.Id);

            entity.Property(n => n.Titulo)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(n => n.Mensagem)
                .IsRequired();

            entity.Property(n => n.DataCriacao)
                .IsRequired();

            entity.Property(n => n.Lida)
                .IsRequired();

            entity.Property(n => n.UsuarioId)
                .IsRequired();
            
            entity.HasOne<Usuario>()
                .WithMany()
                .HasForeignKey(n => n.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        base.OnModelCreating(modelBuilder);
    }
}
