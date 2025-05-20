using CMS.Application.Interfaces;
using CMS.Domain.Entities;

namespace CMS.Application.UseCases.Templates;

public class ListarTemplatesUseCase
{
    private readonly ITemplateRepository _templateRepository;

    public ListarTemplatesUseCase(ITemplateRepository templateRepository)
    {
        _templateRepository = templateRepository;
    }

    public async Task<List<Template>> ExecuteAsync()
    {
        return await _templateRepository.ListarAsync();
    }
}