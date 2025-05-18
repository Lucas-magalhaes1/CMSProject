using CMS.Application.Interfaces;
using CMS.Domain.Entities;

public class ObterUsuarioPorIdUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public ObterUsuarioPorIdUseCase(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<Usuario?> ExecuteAsync(Guid id)
    {
        return await _usuarioRepository.ObterPorIdAsync(id);
    }
}