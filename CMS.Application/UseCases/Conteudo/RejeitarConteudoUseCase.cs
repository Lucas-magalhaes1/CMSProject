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
        if (!_permissaoUsuario.PodeRejeitarConteudo())  
        {
            throw new UnauthorizedAccessException("Você não tem permissão para rejeitar o conteúdo.");
        }

        var conteudo = await _conteudoRepository.ObterPorIdAsync(id);
        if (conteudo == null)
            return null;

        
        var conteudoRejeitado = await _rejeitarConteudoHandler.ManipularConteudo(conteudo, comentario);

        
        await _conteudoRepository.AtualizarAsync(conteudoRejeitado);

        return conteudoRejeitado; 
    }
}