using CMS.Application.Interfaces;
using CMS.Domain.Entities;

namespace CMS.Application.UseCases.Conteudos;

public class ClonarConteudoUseCase
{
    private readonly IConteudoRepository _conteudoRepository;

    public ClonarConteudoUseCase(IConteudoRepository conteudoRepository)
    {
        _conteudoRepository = conteudoRepository;
    }

    public async Task<Conteudo?> ExecuteAsync(Guid id)
    {
        // Buscar o conteúdo original pelo id
        var conteudoOriginal = await _conteudoRepository.ObterPorIdAsync(id);
        if (conteudoOriginal == null)
            return null;

        // Clonar o conteúdo usando o método Clone()
        var conteudoClone = conteudoOriginal.Clone();

        // Salvar o conteúdo clonado no banco de dados
        return await _conteudoRepository.CriarAsync(conteudoClone);
    }
}