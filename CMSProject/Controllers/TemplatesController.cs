using System.Security.Claims;
using CMS.Application.DTOs;
using CMS.Application.UseCases.Templates;
using CMS.Application.Interfaces;
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
    private readonly DeletarTemplateUseCase _deletarTemplateUseCase;
    private readonly IUsuarioRepository _usuarioRepository;

    public TemplatesController(
        CriarTemplateUseCase criarTemplateUseCase,
        ListarTemplatesUseCase listarTemplatesUseCase,
        ObterTemplatePorIdUseCase obterTemplatePorIdUseCase,
        ClonarTemplateUseCase clonarTemplateUseCase,
        DeletarTemplateUseCase deletarTemplateUseCase,
        IUsuarioRepository usuarioRepository)
    {
        _criarTemplateUseCase = criarTemplateUseCase;
        _listarTemplatesUseCase = listarTemplatesUseCase;
        _obterTemplatePorIdUseCase = obterTemplatePorIdUseCase;
        _clonarTemplateUseCase = clonarTemplateUseCase;
        _deletarTemplateUseCase = deletarTemplateUseCase;
        _usuarioRepository = usuarioRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] TemplateDto templateDto)
    {
        try
        {
            var usuarioId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId);
            if (usuario == null)
                return BadRequest(ResponseDto<string>.Falha("Usuário não encontrado"));

            var nomeCriador = usuario.Nome;

            var campos = templateDto.Campos
                .Select(c => new CMS.Domain.ValueObjects.CampoTemplate(c.Nome, c.Tipo, c.Obrigatorio))
                .ToList();

            var template = await _criarTemplateUseCase.ExecuteAsync(templateDto.Nome, campos, usuarioId, nomeCriador);

            var responseDto = new TemplateDto
            {
                Id = template.Id,
                Nome = template.Nome,
                NomeCriador = template.NomeCriador,
                Campos = template.Campos.Select(c => new CampoTemplateDto
                {
                    Nome = c.Nome,
                    Tipo = c.Tipo,
                    Obrigatorio = c.Obrigatorio
                }).ToList()
            };

            return Ok(ResponseDto<TemplateDto>.Ok(responseDto, "Template criado com sucesso"));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<string>.Falha($"Erro interno: {ex.Message}"));
        }
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var templates = await _listarTemplatesUseCase.ExecuteAsync();

        var responseDtos = templates.Select(t => new TemplateDto
        {
            Id = t.Id,
            Nome = t.Nome,
            NomeCriador = t.NomeCriador,
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
            NomeCriador = template.NomeCriador,
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
        try
        {
            var usuarioId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId);
            if (usuario == null)
                return BadRequest(ResponseDto<string>.Falha("Usuário não encontrado"));

            var nomeCriador = usuario.Nome;

            var clone = await _clonarTemplateUseCase.ExecuteAsync(id, usuarioId, nomeCriador);

            if (clone == null)
                return NotFound(ResponseDto<string>.Falha("Template original não encontrado"));

            var responseDto = new TemplateDto
            {
                Id = clone.Id,
                Nome = clone.Nome,
                NomeCriador = clone.NomeCriador,
                Campos = clone.Campos.Select(c => new CampoTemplateDto
                {
                    Nome = c.Nome,
                    Tipo = c.Tipo,
                    Obrigatorio = c.Obrigatorio
                }).ToList()
            };

            return Ok(ResponseDto<TemplateDto>.Ok(responseDto, "Template clonado com sucesso"));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<string>.Falha($"Erro interno: {ex.Message}"));
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Deletar(Guid id)
    {
        try
        {
            var usuarioIdLogado = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var papelString = User.FindFirst(ClaimTypes.Role)?.Value ?? "";

            bool usuarioEhAdmin = papelString == "Admin";

            bool deletado = await _deletarTemplateUseCase.ExecuteAsync(id, usuarioIdLogado, usuarioEhAdmin);

            if (!deletado)
                return NotFound(ResponseDto<string>.Falha("Template não encontrado"));

            return Ok(ResponseDto<string>.Ok(null, "Template deletado com sucesso"));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<string>.Falha($"Erro interno: {ex.Message}"));
        }
    }
}
