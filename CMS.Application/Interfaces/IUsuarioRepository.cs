using CMS.Domain.Entities;

namespace CMS.Application.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> ObterPorIdAsync(Guid id);
    Task<Usuario?> ObterPorEmailAsync(string email);
    Task<List<Usuario>> ListarAsync();
    Task<Usuario> CriarAsync(Usuario usuario);
    Task AtualizarAsync(Usuario usuario);
    Task DeletarAsync(Guid id);
}