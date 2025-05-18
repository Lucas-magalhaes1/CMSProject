using CMS.Application.DTOs;
using CMS.Application.UseCases.Usuarios;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly CriarUsuarioUseCase _criarUsuarioUseCase;

    public UsuariosController(CriarUsuarioUseCase criarUsuarioUseCase)
    {
        _criarUsuarioUseCase = criarUsuarioUseCase;
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
        // A ser implementado nos próximos passos (ObterPorIdUseCase)
        return Ok();
    }
}