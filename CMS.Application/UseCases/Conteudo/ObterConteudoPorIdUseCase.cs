using CMS.Application.Interfaces;
using CMS.Domain.Entities;

namespace CMS.Application.UseCases.Conteudos
{
    public class ObterConteudoPorIdUseCase
    {
        private readonly IConteudoRepository _conteudoRepository;
        private readonly IPermissaoUsuario _permissaoUsuario;

        public ObterConteudoPorIdUseCase(IConteudoRepository conteudoRepository, IPermissaoUsuario permissaoUsuario)
        {
            _conteudoRepository = conteudoRepository;
            _permissaoUsuario = permissaoUsuario;
        }

        public async Task<Conteudo?> ExecuteAsync(Guid id)
        {
            // Verifica se o usuário tem permissão para visualizar o conteúdo
            if (!_permissaoUsuario.PodeObterConteudoPorId())  
            {
                throw new UnauthorizedAccessException("Você não tem permissão para visualizar o conteúdo.");
            }

            return await _conteudoRepository.ObterPorIdAsync(id);
        }
    }
}