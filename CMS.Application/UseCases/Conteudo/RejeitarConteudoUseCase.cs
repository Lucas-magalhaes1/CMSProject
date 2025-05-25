using CMS.Application.Interfaces;
using CMS.Domain.Chain.Handlers;
using CMS.Domain.Entities;

public class RejeitarConteudoUseCase
{
    private readonly IConteudoRepository _conteudoRepository;
    private readonly RejeitarConteudoHandler _rejeitarConteudoHandler;
    private readonly IPermissaoUsuario _permissaoUsuario;

    public RejeitarConteudoUseCase(IConteudoRepository conteudoRepository, RejeitarConteudoHandler rejeitarConteudoHandler, IPermissaoUsuario permissaoUsuario)
    {
        _conteudoRepository = conteudoRepository;
        _rejeitarConteudoHandler = rejeitarConteudoHandler;
        _permissaoUsuario = permissaoUsuario;
    }

    public async Task<Conteudo?> ExecuteAsync(Guid id, string comentario)
    {
        // Verifica se o usuário tem permissão para rejeitar o conteúdo
        if (!_permissaoUsuario.PodeRejeitarConteudo())  
        {
            throw new UnauthorizedAccessException("Você não tem permissão para rejeitar o conteúdo.");
        }

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