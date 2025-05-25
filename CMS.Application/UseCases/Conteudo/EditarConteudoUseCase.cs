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

        public async Task<Conteudo?> ExecuteAsync(Guid id, List<CampoPreenchido> camposPreenchidos)  
        {
            // Verifica se o usuário tem permissão para editar o conteúdo
            if (!_permissaoUsuario.PodeEditarConteudo()) 
            {
                throw new UnauthorizedAccessException("Você não tem permissão para editar o conteúdo.");
            }

            // Obtém o conteúdo pelo ID
            var conteudo = await _conteudoRepository.ObterPorIdAsync(id);
            if (conteudo == null)
                return null;

            // Atualiza o conteúdo com os novos campos preenchidos
            conteudo.AlterarConteudo(camposPreenchidos);
            await _conteudoRepository.AtualizarAsync(conteudo);

            return conteudo;
        }
    }
}