using CMS.Application.Interfaces;
using CMS.Domain.Entities;

public class ListarUsuariosUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public ListarUsuariosUseCase(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<List<Usuario>> ExecuteAsync()
    {
        return await _usuarioRepository.ListarAsync();
    }
}