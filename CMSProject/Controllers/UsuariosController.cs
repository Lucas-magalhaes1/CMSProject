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
        await _criarUsuarioUseCase.ExecuteAsync(dto.Nome!, dto.Email!, dto.Senha!, dto.Papel!);
        return Ok(new { message = "Usuário criado com sucesso." });
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var usuario = await _obterUsuarioPorIdUseCase.ExecuteAsync(id);
        if (usuario == null) return NotFound();
        return Ok(usuario);
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var usuarios = await _listarUsuariosUseCase.ExecuteAsync();
        return Ok(usuarios);
    }

}