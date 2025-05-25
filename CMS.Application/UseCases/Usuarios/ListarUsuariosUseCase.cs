using CMS.Application.DTOs;
using CMS.Application.Interfaces;
using CMS.Domain.Entities;

namespace CMS.Application.UseCases.Usuarios
{
    public class ListarUsuariosUseCase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPermissaoUsuario _permissaoUsuario;

        public ListarUsuariosUseCase(IUsuarioRepository usuarioRepository, IPermissaoUsuario permissaoUsuario)
        {
            _usuarioRepository = usuarioRepository;
            _permissaoUsuario = permissaoUsuario;
        }

        public async Task<ResponseDto<List<UsuarioResponseDto>>> ExecuteAsync()
        {
            if (!_permissaoUsuario.PodeListarUsuarios())  
            {
                return ResponseDto<List<UsuarioResponseDto>>.Falha("Você não tem permissão para listar usuários.");
            }

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
}