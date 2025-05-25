using CMS.Application.Interfaces;
using CMS.Domain.Chain.Handlers;
using CMS.Domain.Entities;

public class DevolverConteudoUseCase
{
    private readonly IConteudoRepository _conteudoRepository;
    private readonly DevolverConteudoHandler _devolverConteudoHandler;
    private readonly IPermissaoUsuario _permissaoUsuario;

    public DevolverConteudoUseCase(IConteudoRepository conteudoRepository, DevolverConteudoHandler devolverConteudoHandler, IPermissaoUsuario permissaoUsuario)
    {
        _conteudoRepository = conteudoRepository;
        _devolverConteudoHandler = devolverConteudoHandler;
        _permissaoUsuario = permissaoUsuario;
    }

    public async Task<Conteudo?> ExecuteAsync(Guid id, string comentario)
    {
        if (!_permissaoUsuario.PodeDevolverConteudo())  
        {
            throw new UnauthorizedAccessException("Você não tem permissão para devolver o conteúdo.");
        }

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