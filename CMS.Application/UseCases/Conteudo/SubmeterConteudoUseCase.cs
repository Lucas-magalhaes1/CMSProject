using CMS.Application.Interfaces;
using CMS.Domain.Chain.Handlers;
using CMS.Domain.Entities;

public class SubmeterConteudoUseCase
{
    private readonly IConteudoRepository _conteudoRepository;
    private readonly SubmeterConteudoHandler _submeterConteudoHandler;
    private readonly IPermissaoUsuario _permissaoUsuario;

    public SubmeterConteudoUseCase(IConteudoRepository conteudoRepository, SubmeterConteudoHandler submeterConteudoHandler, IPermissaoUsuario permissaoUsuario)
    {
        _conteudoRepository = conteudoRepository;
        _submeterConteudoHandler = submeterConteudoHandler;
        _permissaoUsuario = permissaoUsuario;
    }

    public async Task<Conteudo?> ExecuteAsync(Guid id)
    {
        if (!_permissaoUsuario.PodeSubmeterConteudo()) 
        {
            throw new UnauthorizedAccessException("Você não tem permissão para submeter o conteúdo.");
        }

        var conteudo = await _conteudoRepository.ObterPorIdAsync(id);
        if (conteudo == null)
            return null;
        
        conteudo.Status = "Submetido";
        
        await _conteudoRepository.AtualizarAsync(conteudo);
        
        return await _submeterConteudoHandler.ManipularConteudo(conteudo);
    }
}