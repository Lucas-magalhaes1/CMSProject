using CMS.Application.DTOs;
using CMS.Application.Interfaces;
using CMS.Domain.Entities;

namespace CMS.Application.UseCases.Usuarios
{
    public class ObterUsuarioPorIdUseCase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPermissaoUsuario _permissaoUsuario;

        public ObterUsuarioPorIdUseCase(IUsuarioRepository usuarioRepository, IPermissaoUsuario permissaoUsuario)
        {
            _usuarioRepository = usuarioRepository;
            _permissaoUsuario = permissaoUsuario;
        }

        public async Task<ResponseDto<UsuarioResponseDto>> ExecuteAsync(Guid id)
        {
            if (!_permissaoUsuario.PodeObterUsuarioPorId())  
            {
                return ResponseDto<UsuarioResponseDto>.Falha("Você não tem permissão para visualizar o usuário.");
            }

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
}