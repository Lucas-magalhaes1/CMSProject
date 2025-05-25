using CMS.Application.Interfaces;
using CMS.Domain.Entities;

namespace CMS.Application.UseCases.Conteudos
{
    public class ClonarConteudoUseCase
    {
        private readonly IConteudoRepository _conteudoRepository;
        private readonly IPermissaoUsuario _permissaoUsuario;

        public ClonarConteudoUseCase(IConteudoRepository conteudoRepository, IPermissaoUsuario permissaoUsuario)
        {
            _conteudoRepository = conteudoRepository;
            _permissaoUsuario = permissaoUsuario;
        }

        public async Task<Conteudo?> ExecuteAsync(Guid id)
        {
            // Verifica se o usuário tem permissão para clonar o conteúdo
            if (!_permissaoUsuario.PodeClonarConteudo())  
            {
                throw new UnauthorizedAccessException("Você não tem permissão para clonar o conteúdo.");
            }

            // Buscar o conteúdo original pelo id
            var conteudoOriginal = await _conteudoRepository.ObterPorIdAsync(id);
            if (conteudoOriginal == null)
                return null;

            // Clonar o conteúdo usando o método Clone()
            var conteudoClone = conteudoOriginal.Clone();

            // Salvar o conteúdo clonado no banco de dados
            return await _conteudoRepository.CriarAsync(conteudoClone);
        }
    }
}