using CMS.Application.DTOs;
using CMS.Application.Interfaces;
using CMS.Domain.Entities;

namespace CMS.Application.UseCases.Usuarios;

public class ListarUsuariosUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public ListarUsuariosUseCase(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<ResponseDto<List<UsuarioResponseDto>>> ExecuteAsync()
    {
        var usuarios = await _usuarioRepository.ListarAsync();

        var dtos = usuarios.Select(u => new UsuarioResponseDto
        {
            Id = u.Id,
            Nome = u.Nome,
            Email = u.Email,
            Papel = u.Papel.ToString()
        }).ToList();

        return ResponseDto<List<UsuarioResponseDto>>.Ok(dtos);
    }
}