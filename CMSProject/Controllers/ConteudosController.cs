using CMS.Application.DTOs;
using CMS.Application.UseCases.Conteudos;
using CMS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ConteudosController : ControllerBase
{
    private readonly CriarConteudoUseCase _criarConteudoUseCase;
    private readonly ListarConteudosUseCase _listarConteudosUseCase;
    private readonly ObterConteudoPorIdUseCase _obterConteudoPorIdUseCase;
    private readonly EditarConteudoUseCase _editarConteudoUseCase;
    private readonly SubmeterConteudoUseCase _submeterConteudoUseCase;
    private readonly ClonarConteudoUseCase _clonarConteudoUseCase;

    public ConteudosController(
        CriarConteudoUseCase criarConteudoUseCase,
        ListarConteudosUseCase listarConteudosUseCase,
        ObterConteudoPorIdUseCase obterConteudoPorIdUseCase,
        EditarConteudoUseCase editarConteudoUseCase,
        SubmeterConteudoUseCase submeterConteudoUseCase,
        ClonarConteudoUseCase clonarConteudoUseCase)
    {
        _criarConteudoUseCase = criarConteudoUseCase;
        _listarConteudosUseCase = listarConteudosUseCase;
        _obterConteudoPorIdUseCase = obterConteudoPorIdUseCase;
        _editarConteudoUseCase = editarConteudoUseCase;
        _submeterConteudoUseCase = submeterConteudoUseCase;
        _clonarConteudoUseCase = clonarConteudoUseCase;
    }
    
    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] ConteudoDto conteudoDto)
    {
        try
        {
            var camposPreenchidos = conteudoDto.CamposPreenchidos
                .Select(c => new CampoPreenchido(c.Nome, c.Valor))  
                .ToList();

            var conteudo = await _criarConteudoUseCase.ExecuteAsync(conteudoDto.Titulo, conteudoDto.TemplateId, camposPreenchidos);

            var responseDto = new ConteudoDto
            {
                Id = conteudo.Id,
                Titulo = conteudo.Titulo,
                TemplateId = conteudo.Template.Id, 
                Status = conteudo.Status,
                CamposPreenchidos = conteudo.CamposPreenchidos.Select(c => new CampoConteudoDto
                {
                    Nome = c.Nome,
                    Valor = c.Valor
                }).ToList(),
                Comentario = conteudo.Comentario // Inclui o comentário de devolução
            };

            return Ok(ResponseDto<ConteudoDto>.Ok(responseDto, "Conteúdo criado com sucesso"));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ResponseDto<string>.Falha($"Erro: {ex.Message}"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<string>.Falha($"Erro interno: {ex.Message}"));
        }
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var conteudos = await _listarConteudosUseCase.ExecuteAsync(); 

        var responseDtos = conteudos.Select(c => new ConteudoDto
        {
            Id = c.Id,
            Titulo = c.Titulo,
            TemplateId = c.Template.Id,
            Status = c.Status,
            CamposPreenchidos = c.CamposPreenchidos.Select(campo => new CampoConteudoDto
            {
                Nome = campo.Nome,
                Valor = campo.Valor
            }).ToList(),
            Comentario = c.Comentario // Inclui o comentário de devolução
        }).ToList();

        return Ok(ResponseDto<List<ConteudoDto>>.Ok(responseDtos));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var conteudo = await _obterConteudoPorIdUseCase.ExecuteAsync(id);

        if (conteudo == null)
            return NotFound(ResponseDto<string>.Falha("Conteúdo não encontrado"));

        var responseDto = new ConteudoDto
        {
            Id = conteudo.Id,
            Titulo = conteudo.Titulo,
            TemplateId = conteudo.Template.Id,
            Status = conteudo.Status,
            CamposPreenchidos = conteudo.CamposPreenchidos.Select(c => new CampoConteudoDto
            {
                Nome = c.Nome,
                Valor = c.Valor
            }).ToList(),
            Comentario = conteudo.Comentario // Inclui o comentário de devolução
        };

        return Ok(ResponseDto<ConteudoDto>.Ok(responseDto));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Editar(Guid id, [FromBody] ConteudoDto conteudoDto)
    {
        var camposPreenchidos = conteudoDto.CamposPreenchidos.Select(c => new CampoPreenchido(c.Nome, c.Valor)).ToList();

        var conteudo = await _editarConteudoUseCase.ExecuteAsync(id, camposPreenchidos);

        if (conteudo == null)
            return NotFound(ResponseDto<string>.Falha("Conteúdo não encontrado"));

        var responseDto = new ConteudoDto
        {
            Id = conteudo.Id,
            Titulo = conteudo.Titulo,
            TemplateId = conteudo.Template?.Id ?? Guid.Empty,
            Status = conteudo.Status,
            CamposPreenchidos = conteudo.CamposPreenchidos.Select(c => new CampoConteudoDto
            {
                Nome = c.Nome,
                Valor = c.Valor
            }).ToList(),
            Comentario = conteudo.Comentario // Inclui o comentário de devolução
        };

        return Ok(ResponseDto<ConteudoDto>.Ok(responseDto, "Conteúdo editado com sucesso"));
    }

    [HttpPost("{id}/clone")]
    public async Task<IActionResult> Clonar(Guid id)
    {
        var conteudoClonado = await _clonarConteudoUseCase.ExecuteAsync(id);

        if (conteudoClonado == null)
            return NotFound(ResponseDto<string>.Falha("Conteúdo original não encontrado"));

        var responseDto = new ConteudoDto
        {
            Id = conteudoClonado.Id,
            Titulo = conteudoClonado.Titulo,
            TemplateId = conteudoClonado.Template?.Id ?? Guid.Empty,
            Status = conteudoClonado.Status,
            CamposPreenchidos = conteudoClonado.CamposPreenchidos.Select(c => new CampoConteudoDto
            {
                Nome = c.Nome,
                Valor = c.Valor
            }).ToList(),
            Comentario = conteudoClonado.Comentario // Inclui o comentário de devolução
        };

        return Ok(ResponseDto<ConteudoDto>.Ok(responseDto, "Conteúdo clonado com sucesso"));
    }
}
