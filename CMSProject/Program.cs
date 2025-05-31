using System.Security.Claims;
using System.Text;
using CMS.Application.Interfaces;
using CMS.Application.Services;
using CMS.Application.UseCases.Conteudos;
using CMS.Application.UseCases.Templates;
using CMS.Application.UseCases.Usuarios;
using CMS.Domain.Chain.Handlers;
using CMS.Domain.Entities;
using CMS.Domain.Enums;
using CMS.Infrastructure.Data;
using CMS.Infrastructure.Data.Repositories;
using CMS.Infrastructure.Notifications;
using CMS.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "CMSProject", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Informe o token JWT: **Bearer {seu_token}**"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Id = "Bearer", Type = ReferenceType.SecurityScheme }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://localhost:7244")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Registrar DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrando repositórios e use cases básicos (sem dependência complexa)
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IConteudoRepository, ConteudoRepository>();
builder.Services.AddScoped<ITemplateRepository, TemplateRepository>();
builder.Services.AddScoped<INotificacaoRepository, NotificacaoRepository>();

// UseCases e Handlers
builder.Services.AddScoped<CriarUsuarioUseCase>();
builder.Services.AddScoped<ObterUsuarioPorIdUseCase>();
builder.Services.AddScoped<ListarUsuariosUseCase>();
builder.Services.AddScoped<DeletarUsuarioUseCase>();
builder.Services.AddScoped<CriarTemplateUseCase>();
builder.Services.AddScoped<ListarTemplatesUseCase>();
builder.Services.AddScoped<ObterTemplatePorIdUseCase>();
builder.Services.AddScoped<ClonarTemplateUseCase>();
builder.Services.AddScoped<DeletarTemplateUseCase>();
builder.Services.AddScoped<CriarConteudoUseCase>();
builder.Services.AddScoped<ListarConteudosUseCase>();
builder.Services.AddScoped<ObterConteudoPorIdUseCase>();
builder.Services.AddScoped<EditarConteudoUseCase>();
builder.Services.AddScoped<ClonarConteudoUseCase>();
builder.Services.AddScoped<DeletarConteudoUseCase>();
builder.Services.AddScoped<SubmeterConteudoUseCase>();
builder.Services.AddScoped<AprovarConteudoUseCase>();
builder.Services.AddScoped<RejeitarConteudoUseCase>();
builder.Services.AddScoped<DevolverConteudoUseCase>();
builder.Services.AddScoped<SubmeterConteudoHandler>();
builder.Services.AddScoped<AprovarConteudoHandler>();
builder.Services.AddScoped<RejeitarConteudoHandler>();
builder.Services.AddScoped<DevolverConteudoHandler>();
builder.Services.AddScoped<INotificationObserver, ConteudoPublicadoObserver>();
builder.Services.AddScoped<ListarConteudosAprovadosUseCase>();



// Serviços de autenticação
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<LoginUseCase>();

// Notification services como Scoped para evitar erro de ciclo de vida
builder.Services.AddScoped<NotificationPublisher>();
builder.Services.AddScoped<ConteudoPublicadoObserver>();

builder.Services.AddLogging();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IPermissaoUsuario>(provider =>
{
    var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
    var papelUsuarioClaim = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;

    if (Enum.TryParse(papelUsuarioClaim, out PapelUsuario papelUsuario))
    {
        return PermissaoFactory.CriarPermissao(papelUsuario);
    }

    throw new UnauthorizedAccessException("O papel do usuário não foi encontrado no token.");
});

var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["Key"]!))
    };
});

var app = builder.Build();

// Registra observer no publisher dentro do escopo para evitar múltiplas inscrições e problemas
using (var scope = app.Services.CreateScope())
{
    var publisher = scope.ServiceProvider.GetRequiredService<NotificationPublisher>();
    var observer = scope.ServiceProvider.GetRequiredService<ConteudoPublicadoObserver>();
    publisher.Register(observer);

    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    // Criar usuários padrão se não existirem
    var usuariosPadrao = new List<Usuario>
    {
        new Usuario("Admin Padrão", "admin@cms.com", BCrypt.Net.BCrypt.HashPassword("admin123"), PapelUsuario.Admin),
        new Usuario("Editor Padrão", "editor@cms.com", BCrypt.Net.BCrypt.HashPassword("editor123"), PapelUsuario.Editor),
        new Usuario("Redator Padrão", "redator@cms.com", BCrypt.Net.BCrypt.HashPassword("redator123"), PapelUsuario.Redator)
    };

    foreach (var usuarioPadrao in usuariosPadrao)
    {
        var existeUsuario = db.Usuarios.Any(u => u.Email == usuarioPadrao.Email);
        if (!existeUsuario)
        {
            db.Usuarios.Add(usuarioPadrao);
        }
    }

    db.SaveChanges();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
