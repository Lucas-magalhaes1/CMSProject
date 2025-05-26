using System.Security.Claims;
using CMS.Application.DTOs;
using CMS.Application.UseCases.Usuarios;
using CMS.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly CriarUsuarioUseCase _criarUsuarioUseCase;
    private readonly ObterUsuarioPorIdUseCase _obterUsuarioPorIdUseCase;
    private readonly ListarUsuariosUseCase _listarUsuariosUseCase;
    private readonly DeletarUsuarioUseCase _deletarUsuarioUseCase;

    public UsuariosController(
        CriarUsuarioUseCase criarUsuarioUseCase,
        ObterUsuarioPorIdUseCase obterUsuarioPorIdUseCase,
        ListarUsuariosUseCase listarUsuariosUseCase,
        DeletarUsuarioUseCase deletarUsuarioUseCase)
    {
        _criarUsuarioUseCase = criarUsuarioUseCase;
        _obterUsuarioPorIdUseCase = obterUsuarioPorIdUseCase;
        _listarUsuariosUseCase = listarUsuariosUseCase;
        _deletarUsuarioUseCase = deletarUsuarioUseCase;
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Deletar(Guid id)
    {
        try
        {
            var usuarioIdLogado = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var papelString = User.FindFirst(ClaimTypes.Role)?.Value ?? "";
        
            if (!Enum.TryParse(papelString, out PapelUsuario papelUsuarioLogado))
                return Forbid("Papel do usuário inválido.");

            bool deletado = await _deletarUsuarioUseCase.ExecuteAsync(id, usuarioIdLogado, papelUsuarioLogado);

            if (!deletado)
                return NotFound(ResponseDto<string>.Falha("Usuário não encontrado"));

            return Ok(ResponseDto<string>.Ok(null, "Usuário deletado com sucesso"));
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
