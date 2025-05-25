using CMS.Application.Interfaces;
using CMS.Domain.Entities;

namespace CMS.Application.UseCases.Templates
{
    public class ClonarTemplateUseCase
    {
        private readonly ITemplateRepository _templateRepository;
        private readonly IPermissaoUsuario _permissaoUsuario;

        public ClonarTemplateUseCase(ITemplateRepository templateRepository, IPermissaoUsuario permissaoUsuario)
        {
            _templateRepository = templateRepository;
            _permissaoUsuario = permissaoUsuario;
        }

        public async Task<Template?> ExecuteAsync(Guid templateId)
        {
            if (!_permissaoUsuario.PodeClonarTemplate())  
            {
                throw new UnauthorizedAccessException("Você não tem permissão para clonar o template.");
            }

            var templateOriginal = await _templateRepository.ObterPorIdAsync(templateId);
            if (templateOriginal == null)
                return null;

            var clone = templateOriginal.Clone();

            // Opcional: alterar o nome para indicar que é uma cópia
            clone = new Template(clone.Nome + " - Cópia", clone.Campos);

            var novoTemplate = await _templateRepository.CriarAsync(clone);
            return novoTemplate;
        }
    }
}