using CMS.Application.DTOs;
using CMS.Application.UseCases.Usuarios;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly CriarUsuarioUseCase _criarUsuarioUseCase;
    private readonly ObterUsuarioPorIdUseCase _obterUsuarioPorIdUseCase;
    private readonly ListarUsuariosUseCase _listarUsuariosUseCase;

    public UsuariosController(
        CriarUsuarioUseCase criarUsuarioUseCase,
        ObterUsuarioPorIdUseCase obterUsuarioPorIdUseCase,
        ListarUsuariosUseCase listarUsuariosUseCase)
    {
        _criarUsuarioUseCase = criarUsuarioUseCase;
        _obterUsuarioPorIdUseCase = obterUsuarioPorIdUseCase;
        _listarUsuariosUseCase = listarUsuariosUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] UsuarioDto dto)
    {
        var result = await _criarUsuarioUseCase.ExecuteAsync(dto.Nome!, dto.Email!, dto.Senha!, dto.Papel!);

        if (!result.Sucesso)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var result = await _obterUsuarioPorIdUseCase.ExecuteAsync(id);

        if (!result.Sucesso)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var result = await _listarUsuariosUseCase.ExecuteAsync();
        return Ok(result);
    }
}