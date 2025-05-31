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

        public async Task<Conteudo?> ExecuteAsync(Guid id, Guid usuarioId)
        {
            
            var conteudoOriginal = await _conteudoRepository.ObterPorIdAsync(id);
            if (conteudoOriginal == null)
                return null;

            
            if (conteudoOriginal.CriadoPor != usuarioId && !_permissaoUsuario.PodeAprovarConteudo()) 
            {
                throw new UnauthorizedAccessException("Você não tem permissão para clonar este conteúdo.");
            }

            
            var conteudoClone = conteudoOriginal.Clone();

            
            return await _conteudoRepository.CriarAsync(conteudoClone);
        }
    }
}