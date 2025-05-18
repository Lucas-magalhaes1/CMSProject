using CMS.Application.Interfaces;
using CMS.Domain.Entities;
using BCrypt.Net;

namespace CMS.Application.UseCases.Usuarios;

public class CriarUsuarioUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public CriarUsuarioUseCase(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task ExecuteAsync(string nome, string email, string senhaPura, string papel)
    {
        if (string.IsNullOrWhiteSpace(nome)) throw new ArgumentException("Nome é obrigatório");
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email é obrigatório");
        if (string.IsNullOrWhiteSpace(senhaPura)) throw new ArgumentException("Senha é obrigatória");
        if (string.IsNullOrWhiteSpace(papel)) throw new ArgumentException("Papel é obrigatório");
        
        // Gera o hash da senha
        var senhaHash = BCrypt.Net.BCrypt.HashPassword(senhaPura);

        var usuario = new Usuario(nome, email, senhaHash, papel);
        await _usuarioRepository.CriarAsync(usuario);
    }
}