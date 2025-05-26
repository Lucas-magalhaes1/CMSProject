using CMS.Application.Interfaces;
using CMS.Domain.Entities;

namespace CMS.Application.UseCases.Conteudos
{
    public class ListarConteudosUseCase
    {
        private readonly IConteudoRepository _conteudoRepository;
        private readonly IPermissaoUsuario _permissaoUsuario;

        public ListarConteudosUseCase(IConteudoRepository conteudoRepository, IPermissaoUsuario permissaoUsuario)
        {
            _conteudoRepository = conteudoRepository;
            _permissaoUsuario = permissaoUsuario;
        }

        public async Task<List<Conteudo>> ExecuteAsync(Guid usuarioId)
        {
            // Se o usuário é um admin ou editor, pode listar todos os conteúdos
            if (_permissaoUsuario.PodeAprovarConteudo())  // Admin ou Editor
            {
                return await _conteudoRepository.ListarAsync();
            }
            
            // Caso contrário, apenas o autor pode listar seus próprios conteúdos
            return await _conteudoRepository.ListarPorCriadorAsync(usuarioId); // Método para listar apenas os conteúdos criados pelo usuário
        }
    }
}