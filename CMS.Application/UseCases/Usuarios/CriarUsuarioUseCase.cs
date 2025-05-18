using CMS.Application.Interfaces;
using CMS.Domain.Entities;

namespace CMS.Application.UseCases.Usuarios;

public class CriarUsuarioUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public CriarUsuarioUseCase(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task ExecuteAsync(string nome, string email, string senhaHash, string papel)
    {
        var usuario = new Usuario(nome, email, senhaHash, papel);
        await _usuarioRepository.CriarAsync(usuario);
    }
}