using CMS.Application.Interfaces;
using CMS.Domain.Entities;
using CMS.Domain.Enums;

public class DeletarUsuarioUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPermissaoUsuario _permissaoUsuario;

    public DeletarUsuarioUseCase(IUsuarioRepository usuarioRepository, IPermissaoUsuario permissaoUsuario)
    {
        _usuarioRepository = usuarioRepository;
        _permissaoUsuario = permissaoUsuario;
    }

    public async Task<bool> ExecuteAsync(Guid idUsuarioParaDeletar, Guid idUsuarioLogado, PapelUsuario papelUsuarioLogado)
    {
        // Só o próprio usuário ou admin podem deletar
        bool usuarioEhAdmin = papelUsuarioLogado == PapelUsuario.Admin;
        bool usuarioEhProprio = idUsuarioParaDeletar == idUsuarioLogado;

        if (!usuarioEhAdmin && !usuarioEhProprio)
        {
            throw new UnauthorizedAccessException("Você não tem permissão para deletar este usuário.");
        }

        var usuario = await _usuarioRepository.ObterPorIdAsync(idUsuarioParaDeletar);
        if (usuario == null)
            return false;

        await _usuarioRepository.DeletarAsync(usuario);
        return true;
    }
}