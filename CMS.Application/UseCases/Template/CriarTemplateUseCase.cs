using CMS.Application.Interfaces;
using CMS.Domain.Entities;
using CMS.Domain.ValueObjects;

namespace CMS.Application.UseCases.Templates;

public class CriarTemplateUseCase
{
    private readonly ITemplateRepository _templateRepository;

    public CriarTemplateUseCase(ITemplateRepository templateRepository)
    {
        _templateRepository = templateRepository;
    }

    public async Task<Template> ExecuteAsync(string nome, List<CampoTemplate> campos)
    {
        var template = new Template(nome, campos);
        return await _templateRepository.CriarAsync(template);
    }
}