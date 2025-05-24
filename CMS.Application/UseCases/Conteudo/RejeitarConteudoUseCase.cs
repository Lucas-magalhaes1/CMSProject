using CMS.Application.Interfaces;
using CMS.Domain.Chain.Handlers;
using CMS.Domain.Entities;

public class RejeitarConteudoUseCase
{
    private readonly IConteudoRepository _conteudoRepository;
    private readonly RejeitarConteudoHandler _rejeitarConteudoHandler;

    public RejeitarConteudoUseCase(IConteudoRepository conteudoRepository, RejeitarConteudoHandler rejeitarConteudoHandler)
    {
        _conteudoRepository = conteudoRepository;
        _rejeitarConteudoHandler = rejeitarConteudoHandler;
    }

    public async Task<Conteudo?> ExecuteAsync(Guid id, string comentario)
    {
        var conteudo = await _conteudoRepository.ObterPorIdAsync(id);
        if (conteudo == null)
            return null;

        // Passa o conteúdo e o comentário para o handler de rejeição, que altera o status e o comentário
        var conteudoRejeitado = await _rejeitarConteudoHandler.ManipularConteudo(conteudo, comentario);

        // Agora salvamos a alteração no banco de dados, incluindo o comentário
        await _conteudoRepository.AtualizarAsync(conteudoRejeitado);

        return conteudoRejeitado; // Retorna o conteúdo rejeitado com o status alterado e o comentário
    }
}