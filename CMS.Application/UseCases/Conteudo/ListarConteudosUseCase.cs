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

        public async Task<List<Conteudo>> ExecuteAsync()
        {
            // Verifica se o usuário tem permissão para visualizar o conteúdo
            if (!_permissaoUsuario.PodeListarConteudos())  // Ajuste para a permissão correta
            {
                throw new UnauthorizedAccessException("Você não tem permissão para listar o conteúdo.");
            }

            return await _conteudoRepository.ListarAsync();
        }
    }
}