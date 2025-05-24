using CMS.Application.DTOs;
using CMS.Application.UseCases.Conteudos;
using CMS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CMS.API.Controllers;

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

            // Executando a lógica de criação do conteúdo
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
                }).ToList()
            };

            // Retorna um sucesso
            return Ok(ResponseDto<ConteudoDto>.Ok(responseDto, "Conteúdo criado com sucesso"));
        }
        catch (ArgumentException ex)
        {
            // Retorna falha com o código e mensagem do erro
            return BadRequest(ResponseDto<string>.Falha($"Erro: {ex.Message}"));
        }
        catch (Exception ex)
        {
            // Retorna falha para outros tipos de erro
            return StatusCode(500, ResponseDto<string>.Falha($"Erro interno: {ex.Message}"));
        }
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        // Utilizando o UseCase já injetado, que já faz a chamada ao repositório
        var conteudos = await _listarConteudosUseCase.ExecuteAsync(); // Chamando o caso de uso

        var responseDtos = conteudos.Select(c => new ConteudoDto
        {
            Id = c.Id,
            Titulo = c.Titulo,
            TemplateId = c.Template.Id,  // Acessa o TemplateId corretamente
            Status = c.Status,
            CamposPreenchidos = c.CamposPreenchidos.Select(campo => new CampoConteudoDto
            {
                Nome = campo.Nome,
                Valor = campo.Valor
            }).ToList()
        }).ToList();

        return Ok(ResponseDto<List<ConteudoDto>>.Ok(responseDtos));
    }

// Obter conteúdo por ID
    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var conteudo = await _obterConteudoPorIdUseCase.ExecuteAsync(id);
    
        // Se o conteúdo não for encontrado, retorna um erro
        if (conteudo == null)
            return NotFound(ResponseDto<string>.Falha("Conteúdo não encontrado"));

        // Monta o DTO para resposta
        var responseDto = new ConteudoDto
        {
            Id = conteudo.Id,
            Titulo = conteudo.Titulo,
            TemplateId = conteudo.Template.Id,  // Acessa diretamente o TemplateId, pois Template nunca é null
            Status = conteudo.Status,
            CamposPreenchidos = conteudo.CamposPreenchidos.Select(c => new CampoConteudoDto
            {
                Nome = c.Nome,  // Acessando diretamente a propriedade Nome
                Valor = c.Valor // Acessando diretamente a propriedade Valor
            }).ToList()
        };

        // Retorna a resposta com sucesso
        return Ok(ResponseDto<ConteudoDto>.Ok(responseDto));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Editar(Guid id, [FromBody] ConteudoDto conteudoDto)
    {
        var camposPreenchidos = conteudoDto.CamposPreenchidos.Select(c => new CampoPreenchido(c.Nome, c.Valor)).ToList();
    
        // Garantir que o Template seja carregado ao buscar o conteúdo
        var conteudo = await _editarConteudoUseCase.ExecuteAsync(id, camposPreenchidos);

        if (conteudo == null)
            return NotFound(ResponseDto<string>.Falha("Conteúdo não encontrado"));

        var responseDto = new ConteudoDto
        {
            Id = conteudo.Id,
            Titulo = conteudo.Titulo,
            TemplateId = conteudo.Template?.Id ?? Guid.Empty, // Acessa diretamente o TemplateId
            Status = conteudo.Status,
            CamposPreenchidos = conteudo.CamposPreenchidos.Select(c => new CampoConteudoDto
            {
                Nome = c.Nome,
                Valor = c.Valor
            }).ToList()
        };

        return Ok(ResponseDto<ConteudoDto>.Ok(responseDto, "Conteúdo editado com sucesso"));
    }


    [HttpPost("{id}/submeter")]
    public async Task<IActionResult> Submeter(Guid id)
    {
        var conteudo = await _submeterConteudoUseCase.ExecuteAsync(id);

        if (conteudo == null)
            return NotFound(ResponseDto<string>.Falha("Conteúdo não encontrado"));

        var responseDto = new ConteudoDto
        {
            Id = conteudo.Id,
            Titulo = conteudo.Titulo,
            TemplateId = conteudo.Template?.Id ?? Guid.Empty, // Acessa diretamente o TemplateId
            Status = conteudo.Status,
            CamposPreenchidos = conteudo.CamposPreenchidos.Select(c => new CampoConteudoDto
            {
                Nome = c.Nome,
                Valor = c.Valor
            }).ToList()
        };

        return Ok(ResponseDto<ConteudoDto>.Ok(responseDto, "Conteúdo submetido para revisão"));
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
            TemplateId = conteudoClonado.Template?.Id ?? Guid.Empty, // Acessa diretamente o TemplateId
            Status = conteudoClonado.Status,
            CamposPreenchidos = conteudoClonado.CamposPreenchidos.Select(c => new CampoConteudoDto
            {
                Nome = c.Nome,
                Valor = c.Valor
            }).ToList()
        };

        return Ok(ResponseDto<ConteudoDto>.Ok(responseDto, "Conteúdo clonado com sucesso"));
    }
}
