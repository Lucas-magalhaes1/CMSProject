using CMS.Application.DTOs;
using CMS.Application.Interfaces;
using CMS.Domain.Entities;

namespace CMS.Application.UseCases.Usuarios;

public class LoginUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IAuthService _authService;

    public LoginUseCase(IUsuarioRepository usuarioRepository, IAuthService authService)
    {
        _usuarioRepository = usuarioRepository;
        _authService = authService;
    }

    public async Task<ResponseDto<LoginResponseDto>> ExecuteAsync(LoginDto dto)
    {
        var usuario = await _usuarioRepository.ObterPorEmailAsync(dto.Email);
        if (usuario == null)
            return ResponseDto<LoginResponseDto>.Falha("Usuário ou senha inválidos");

        var senhaValida = BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash);
        if (!senhaValida)
            return ResponseDto<LoginResponseDto>.Falha("Usuário ou senha inválidos");

        var token = _authService.GerarToken(usuario.Id, usuario.Papel.ToString());

        var response = new LoginResponseDto
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Papel = usuario.Papel.ToString(),
            Token = token
        };

        return ResponseDto<LoginResponseDto>.Ok(response, "Login realizado com sucesso");
    }
}