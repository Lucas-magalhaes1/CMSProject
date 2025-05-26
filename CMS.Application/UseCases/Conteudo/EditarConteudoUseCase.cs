using CMS.Application.Interfaces;
using CMS.Domain.Entities;

namespace CMS.Application.UseCases.Conteudos
{
    public class EditarConteudoUseCase
    {
        private readonly IConteudoRepository _conteudoRepository;
        private readonly IPermissaoUsuario _permissaoUsuario;

        public EditarConteudoUseCase(IConteudoRepository conteudoRepository, IPermissaoUsuario permissaoUsuario)
        {
            _conteudoRepository = conteudoRepository;
            _permissaoUsuario = permissaoUsuario;
        }

        public async Task<Conteudo?> ExecuteAsync(Guid id, List<CampoPreenchido> camposPreenchidos, Guid usuarioId)
        {
            // Obtém o conteúdo pelo ID
            var conteudo = await _conteudoRepository.ObterPorIdAsync(id);
            if (conteudo == null)
                return null;

            // Verifica se o conteúdo é do usuário ou se ele tem permissão para editar (admin/editor)
            if (conteudo.CriadoPor != usuarioId && !_permissaoUsuario.PodeEditarConteudo()) // Apenas o criador ou admin/editor
            {
                throw new UnauthorizedAccessException("Você não tem permissão para editar este conteúdo.");
            }

            // Atualiza o conteúdo com os novos campos preenchidos
            conteudo.AlterarConteudo(camposPreenchidos);
            await _conteudoRepository.AtualizarAsync(conteudo);

            return conteudo;
        }
    }
}