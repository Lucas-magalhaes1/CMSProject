using CMS.Application.Interfaces;
using CMS.Domain.Entities;
using System;

namespace CMS.Application.UseCases.Templates;

public class ObterTemplatePorIdUseCase
{
    private readonly ITemplateRepository _templateRepository;

    public ObterTemplatePorIdUseCase(ITemplateRepository templateRepository)
    {
        _templateRepository = templateRepository;
    }

    public async Task<Template?> ExecuteAsync(Guid id)
    {
        return await _templateRepository.ObterPorIdAsync(id);
    }
}