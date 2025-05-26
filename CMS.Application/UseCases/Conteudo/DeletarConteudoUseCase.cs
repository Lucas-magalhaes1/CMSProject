using CMS.Application.Interfaces;
using CMS.Domain.Entities;

public class DeletarConteudoUseCase
{
    private readonly IConteudoRepository _conteudoRepository;
    private readonly IPermissaoUsuario _permissaoUsuario;

    public DeletarConteudoUseCase(IConteudoRepository conteudoRepository, IPermissaoUsuario permissaoUsuario)
    {
        _conteudoRepository = conteudoRepository;
        _permissaoUsuario = permissaoUsuario;
    }

    public async Task<bool> ExecuteAsync(Guid id, Guid usuarioId)
    {
        if (!_permissaoUsuario.PodeDeletarConteudo())
            throw new UnauthorizedAccessException("Você não tem permissão para deletar conteúdos.");

        var conteudo = await _conteudoRepository.ObterPorIdAsync(id);
        if (conteudo == null)
            return false;

        // Verifica se o usuário é o criador ou tem permissão especial (ex: Admin)
        if (conteudo.CriadoPor != usuarioId && !_permissaoUsuario.PodeAprovarConteudo())
        {
            // PodeAprovarConteudo() usada aqui como proxy para 'é admin' — ajuste conforme sua permissão real de admin
            throw new UnauthorizedAccessException("Você só pode deletar conteúdos que criou ou se for administrador.");
        }

        await _conteudoRepository.DeletarAsync(conteudo);
        return true;
    }
}