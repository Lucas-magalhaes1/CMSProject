using CMS.Application.Interfaces;
using CMS.Domain.Chain.Handlers;
using CMS.Domain.Entities;

public class AprovarConteudoUseCase
{
    private readonly IConteudoRepository _conteudoRepository;
    private readonly AprovarConteudoHandler _aprovarConteudoHandler;
    private readonly IPermissaoUsuario _permissaoUsuario;

    public AprovarConteudoUseCase(IConteudoRepository conteudoRepository, AprovarConteudoHandler aprovarConteudoHandler, IPermissaoUsuario permissaoUsuario)
    {
        _conteudoRepository = conteudoRepository;
        _aprovarConteudoHandler = aprovarConteudoHandler;
        _permissaoUsuario = permissaoUsuario;
    }

    public async Task<Conteudo?> ExecuteAsync(Guid id)
    {
        // Verifica se o usuário tem permissão para aprovar o conteúdo
        if (!_permissaoUsuario.PodeAprovarConteudo())
        {
            throw new UnauthorizedAccessException("Você não tem permissão para aprovar conteúdo.");
        }

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