using System.Text;
using CMS.Application.Interfaces;
using CMS.Application.UseCases.Conteudos;
using CMS.Application.UseCases.Templates;
using CMS.Application.UseCases.Usuarios;
using CMS.Domain.Chain.Handlers;
using CMS.Infrastructure.Data;
using CMS.Infrastructure.Data.Repositories;
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
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<CriarUsuarioUseCase>();
builder.Services.AddScoped<ObterUsuarioPorIdUseCase>();
builder.Services.AddScoped<ListarUsuariosUseCase>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<LoginUseCase>();


builder.Services.AddScoped<ITemplateRepository, TemplateRepository>();
builder.Services.AddScoped<CriarTemplateUseCase>();
builder.Services.AddScoped<ListarTemplatesUseCase>();
builder.Services.AddScoped<ObterTemplatePorIdUseCase>();
builder.Services.AddScoped<ClonarTemplateUseCase>();

builder.Services.AddScoped<IConteudoRepository, ConteudoRepository>();
builder.Services.AddScoped<CriarConteudoUseCase>();
builder.Services.AddScoped<ListarConteudosUseCase>();
builder.Services.AddScoped<ObterConteudoPorIdUseCase>();
builder.Services.AddScoped<EditarConteudoUseCase>();
builder.Services.AddScoped<ClonarConteudoUseCase>();

builder.Services.AddScoped<SubmeterConteudoUseCase>();
builder.Services.AddScoped<AprovarConteudoUseCase>();
builder.Services.AddScoped<RejeitarConteudoUseCase>();
builder.Services.AddScoped<DevolverConteudoUseCase>();


builder.Services.AddScoped<SubmeterConteudoHandler>();
builder.Services.AddScoped<AprovarConteudoHandler>();
builder.Services.AddScoped<RejeitarConteudoHandler>();
builder.Services.AddScoped<DevolverConteudoHandler>();



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


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); 
app.UseAuthorization();
app.MapControllers();
app.Run();
