using CMS.Application.Interfaces;
using CMS.Domain.Entities;

namespace CMS.Application.UseCases.Templates;

public class ClonarTemplateUseCase
{
    private readonly ITemplateRepository _templateRepository;

    public ClonarTemplateUseCase(ITemplateRepository templateRepository)
    {
        _templateRepository = templateRepository;
    }

    public async Task<Template?> ExecuteAsync(Guid templateId)
    {
        var templateOriginal = await _templateRepository.ObterPorIdAsync(templateId);
        if (templateOriginal == null)
            return null;

        var clone = templateOriginal.Clone();

        // Opcional: alterar o nome para indicar que é uma cópia
        clone = new Template(clone.Nome + " - Cópia", clone.Campos);

        var novoTemplate = await _templateRepository.CriarAsync(clone);
        return novoTemplate;
    }
}