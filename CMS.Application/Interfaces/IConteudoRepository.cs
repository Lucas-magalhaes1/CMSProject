using CMS.Domain.Entities;

namespace CMS.Application.Interfaces;

public interface IConteudoRepository
{
    Task<Conteudo> CriarAsync(Conteudo conteudo);
    Task<List<Conteudo>> ListarAsync();
    Task<Conteudo?> ObterPorIdAsync(Guid id);
    Task AtualizarAsync(Conteudo conteudo);
}