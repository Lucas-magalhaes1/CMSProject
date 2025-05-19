using CMS.Application.DTOs;
using CMS.Application.Interfaces;
using CMS.Domain.Entities;

namespace CMS.Application.UseCases.Usuarios;

public class ObterUsuarioPorIdUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public ObterUsuarioPorIdUseCase(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<ResponseDto<UsuarioResponseDto>> ExecuteAsync(Guid id)
    {
        var usuario = await _usuarioRepository.ObterPorIdAsync(id);
        if (usuario == null)
            return ResponseDto<UsuarioResponseDto>.Falha("Usuário não encontrado");

        var dto = new UsuarioResponseDto
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Papel = usuario.Papel.ToString()
        };

        return ResponseDto<UsuarioResponseDto>.Ok(dto);
    }
}