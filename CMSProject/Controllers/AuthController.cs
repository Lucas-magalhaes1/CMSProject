using CMS.Application.DTOs;
using CMS.Application.UseCases.Usuarios;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly LoginUseCase _loginUseCase;

    public AuthController(LoginUseCase loginUseCase)
    {
        _loginUseCase = loginUseCase;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await _loginUseCase.ExecuteAsync(dto);

        if (!result.Sucesso)
            return Unauthorized(result); 

        return Ok(result); 
    }
}