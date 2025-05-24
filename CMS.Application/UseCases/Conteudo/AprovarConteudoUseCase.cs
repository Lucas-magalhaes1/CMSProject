using CMS.Application.Interfaces;
using CMS.Domain.Chain.Handlers;
using CMS.Domain.Entities;

public class AprovarConteudoUseCase
{
    private readonly IConteudoRepository _conteudoRepository;
    private readonly AprovarConteudoHandler _aprovarConteudoHandler;

    public AprovarConteudoUseCase(IConteudoRepository conteudoRepository, AprovarConteudoHandler aprovarConteudoHandler)
    {
        _conteudoRepository = conteudoRepository;
        _aprovarConteudoHandler = aprovarConteudoHandler;
    }

    public async Task<Conteudo?> ExecuteAsync(Guid id)
    {
        var conteudo = await _conteudoRepository.ObterPorIdAsync(id);
        if (conteudo == null)
            return null;

        // Passa o conteúdo para o handler de aprovação
        var conteudoAprovado = await _aprovarConteudoHandler.ManipularConteudo(conteudo);

        // Salva a alteração no banco de dados
        await _conteudoRepository.AtualizarAsync(conteudoAprovado);

        return conteudoAprovado;
    }
}