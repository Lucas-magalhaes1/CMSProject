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
            // Buscar o conteúdo original pelo id
            var conteudoOriginal = await _conteudoRepository.ObterPorIdAsync(id);
            if (conteudoOriginal == null)
                return null;

            // Verifica se o conteúdo é do usuário ou se ele tem permissão para clonar (admin/editor)
            if (conteudoOriginal.CriadoPor != usuarioId && !_permissaoUsuario.PodeAprovarConteudo()) // Apenas o criador ou admin/editor
            {
                throw new UnauthorizedAccessException("Você não tem permissão para clonar este conteúdo.");
            }

            // Clonar o conteúdo usando o método Clone()
            var conteudoClone = conteudoOriginal.Clone();

            // Salvar o conteúdo clonado no banco de dados
            return await _conteudoRepository.CriarAsync(conteudoClone);
        }
    }
}