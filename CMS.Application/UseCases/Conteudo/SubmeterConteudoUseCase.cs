using CMS.Application.Interfaces;
using CMS.Domain.Entities;

namespace CMS.Application.UseCases.Conteudos;

public class SubmeterConteudoUseCase
{
    private readonly IConteudoRepository _conteudoRepository;

    public SubmeterConteudoUseCase(IConteudoRepository conteudoRepository)
    {
        _conteudoRepository = conteudoRepository;
    }

    public async Task<Conteudo?> ExecuteAsync(Guid id)
    {
        var conteudo = await _conteudoRepository.ObterPorIdAsync(id);
        if (conteudo == null)
            return null;

        // Submete o conteúdo para revisão
        conteudo.Submeter();
        await _conteudoRepository.AtualizarAsync(conteudo);

        return conteudo;
    }
}