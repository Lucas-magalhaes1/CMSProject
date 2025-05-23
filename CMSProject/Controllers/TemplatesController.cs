using CMS.Application.DTOs;
using CMS.Application.UseCases.Templates;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TemplatesController : ControllerBase
{
    private readonly CriarTemplateUseCase _criarTemplateUseCase;
    private readonly ListarTemplatesUseCase _listarTemplatesUseCase;
    private readonly ObterTemplatePorIdUseCase _obterTemplatePorIdUseCase;
    private readonly ClonarTemplateUseCase _clonarTemplateUseCase;

    public TemplatesController(
        CriarTemplateUseCase criarTemplateUseCase,
        ListarTemplatesUseCase listarTemplatesUseCase,
        ObterTemplatePorIdUseCase obterTemplatePorIdUseCase,
        ClonarTemplateUseCase clonarTemplateUseCase
        )
    {
        _criarTemplateUseCase = criarTemplateUseCase;
        _listarTemplatesUseCase = listarTemplatesUseCase;
        _obterTemplatePorIdUseCase = obterTemplatePorIdUseCase;
        _clonarTemplateUseCase = clonarTemplateUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] TemplateDto templateDto)
    {
        // Converter DTO para entidade
        var campos = templateDto.Campos.Select(c => new CMS.Domain.ValueObjects.CampoTemplate(c.Nome, c.Tipo, c.Obrigatorio)).ToList();
        var template = await _criarTemplateUseCase.ExecuteAsync(templateDto.Nome, campos);

        // Converter entidade para DTO para resposta
        var responseDto = new TemplateDto
        {
            Id = template.Id,
            Nome = template.Nome,
            Campos = template.Campos.Select(c => new CampoTemplateDto
            {
                Nome = c.Nome,
                Tipo = c.Tipo,
                Obrigatorio = c.Obrigatorio
            }).ToList()
        };

        return Ok(ResponseDto<TemplateDto>.Ok(responseDto, "Template criado com sucesso"));
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var templates = await _listarTemplatesUseCase.ExecuteAsync();

        var responseDtos = templates.Select(t => new TemplateDto
        {
            Id = t.Id,
            Nome = t.Nome,
            Campos = t.Campos.Select(c => new CampoTemplateDto
            {
                Nome = c.Nome,
                Tipo = c.Tipo,
                Obrigatorio = c.Obrigatorio
            }).ToList()
        }).ToList();

        return Ok(ResponseDto<List<TemplateDto>>.Ok(responseDtos));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var template = await _obterTemplatePorIdUseCase.ExecuteAsync(id);
        if (template == null)
            return NotFound(ResponseDto<string>.Falha("Template não encontrado"));

        var responseDto = new TemplateDto
        {
            Id = template.Id,
            Nome = template.Nome,
            Campos = template.Campos.Select(c => new CampoTemplateDto
            {
                Nome = c.Nome,
                Tipo = c.Tipo,
                Obrigatorio = c.Obrigatorio
            }).ToList()
        };

        return Ok(ResponseDto<TemplateDto>.Ok(responseDto));
    }
    
    [HttpPost("{id}/clone")]
    public async Task<IActionResult> Clonar(Guid id)
    {
        var clone = await _clonarTemplateUseCase.ExecuteAsync(id);
        if (clone == null)
            return NotFound(ResponseDto<string>.Falha("Template original não encontrado"));

        var responseDto = new TemplateDto
        {
            Id = clone.Id,
            Nome = clone.Nome,
            Campos = clone.Campos.Select(c => new CampoTemplateDto
            {
                Nome = c.Nome,
                Tipo = c.Tipo,
                Obrigatorio = c.Obrigatorio
            }).ToList()
        };

        return Ok(ResponseDto<TemplateDto>.Ok(responseDto, "Template clonado com sucesso"));
    }
}
