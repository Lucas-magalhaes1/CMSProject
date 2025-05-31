using CMS.Application.DTOs;
using CMS.Application.UseCases.Conteudos;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AprovacaoConteudoController : ControllerBase
{
    private readonly AprovarConteudoUseCase _aprovarConteudoUseCase;
    private readonly RejeitarConteudoUseCase _rejeitarConteudoUseCase;
    private readonly DevolverConteudoUseCase _devolverConteudoUseCase;
    private readonly SubmeterConteudoUseCase _submeterConteudoUseCase;

    public AprovacaoConteudoController(
        AprovarConteudoUseCase aprovarConteudoUseCase,
        RejeitarConteudoUseCase rejeitarConteudoUseCase,
        DevolverConteudoUseCase devolverConteudoUseCase,
        SubmeterConteudoUseCase submeterConteudoUseCase)
    {
        _aprovarConteudoUseCase = aprovarConteudoUseCase;
        _rejeitarConteudoUseCase = rejeitarConteudoUseCase;
        _devolverConteudoUseCase = devolverConteudoUseCase;
        _submeterConteudoUseCase = submeterConteudoUseCase;
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
            TemplateId = conteudo.Template?.Id ?? Guid.Empty,
            Status = conteudo.Status,
            CamposPreenchidos = conteudo.CamposPreenchidos.Select(c => new CampoConteudoDto
            {
                Nome = c.Nome,
                Valor = c.Valor
            }).ToList(),
            Comentario = conteudo.Comentario 
        };

        return Ok(ResponseDto<ConteudoDto>.Ok(responseDto, "Conteúdo submetido para revisão"));
    }

    
    [HttpPost("{id}/aprovar")]
    public async Task<IActionResult> Aprovar(Guid id)
    {
        var conteudo = await _aprovarConteudoUseCase.ExecuteAsync(id);
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
            Comentario = conteudo.Comentario 
        };

        return Ok(ResponseDto<ConteudoDto>.Ok(responseDto, "Conteúdo aprovado"));
    }

    
    [HttpPost("{id}/rejeitar")]
    public async Task<IActionResult> Rejeitar(Guid id, [FromBody] string comentario)
    {
        var conteudo = await _rejeitarConteudoUseCase.ExecuteAsync(id, comentario);  
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
            Comentario = conteudo.Comentario 
        };

        return Ok(ResponseDto<ConteudoDto>.Ok(responseDto, "Conteúdo rejeitado"));
    }

    
    [HttpPost("{id}/devolver")]
    public async Task<IActionResult> DevolverConteudo(Guid id, [FromBody] string comentario)
    {
        var conteudo = await _devolverConteudoUseCase.ExecuteAsync(id, comentario);

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
            Comentario = conteudo.Comentario 
        };

        return Ok(ResponseDto<ConteudoDto>.Ok(responseDto, "Conteúdo devolvido para correção"));
    }
}
