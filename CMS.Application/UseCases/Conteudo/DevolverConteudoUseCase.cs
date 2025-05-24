using CMS.Application.Interfaces;
using CMS.Domain.Chain.Handlers;
using CMS.Domain.Entities;

public class DevolverConteudoUseCase
{
    private readonly IConteudoRepository _conteudoRepository;
    private readonly DevolverConteudoHandler _devolverConteudoHandler;

    public DevolverConteudoUseCase(IConteudoRepository conteudoRepository, DevolverConteudoHandler devolverConteudoHandler)
    {
        _conteudoRepository = conteudoRepository;
        _devolverConteudoHandler = devolverConteudoHandler;
    }

    public async Task<Conteudo?> ExecuteAsync(Guid id, string comentario)
    {
        var conteudo = await _conteudoRepository.ObterPorIdAsync(id);
        if (conteudo == null)
            return null;

        // Passa o conteúdo para o handler de devolução com o comentário de correção
        var conteudoDevolvido = await _devolverConteudoHandler.ManipularConteudo(conteudo, comentario);

        // Salva a alteração no banco de dados
        await _conteudoRepository.AtualizarAsync(conteudoDevolvido);

        return conteudoDevolvido;
    }
}