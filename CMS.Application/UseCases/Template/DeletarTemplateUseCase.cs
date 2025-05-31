using CMS.Application.Interfaces;
using CMS.Domain.Entities;

public class DeletarTemplateUseCase
{
    private readonly ITemplateRepository _templateRepository;
    private readonly IPermissaoUsuario _permissaoUsuario;

    public DeletarTemplateUseCase(ITemplateRepository templateRepository, IPermissaoUsuario permissaoUsuario)
    {
        _templateRepository = templateRepository;
        _permissaoUsuario = permissaoUsuario;
    }

    public async Task<bool> ExecuteAsync(Guid id, Guid usuarioId, bool usuarioEhAdmin)
    {
        var template = await _templateRepository.ObterPorIdAsync(id);
        if (template == null)
            return false;

        if (!usuarioEhAdmin && template.CriadoPor != usuarioId)
        {
            throw new UnauthorizedAccessException("Você não tem permissão para deletar este template.");
        }

        await _templateRepository.DeletarAsync(template);
        return true;
    }
}