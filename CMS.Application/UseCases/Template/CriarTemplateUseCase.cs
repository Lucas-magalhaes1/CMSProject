using CMS.Application.Interfaces;
using CMS.Domain.Entities;
using CMS.Domain.ValueObjects;

namespace CMS.Application.UseCases.Templates
{
    public class CriarTemplateUseCase
    {
        private readonly ITemplateRepository _templateRepository;
        private readonly IPermissaoUsuario _permissaoUsuario;

        public CriarTemplateUseCase(ITemplateRepository templateRepository, IPermissaoUsuario permissaoUsuario)
        {
            _templateRepository = templateRepository;
            _permissaoUsuario = permissaoUsuario;
        }

        public async Task<Template> ExecuteAsync(string nome, List<CampoTemplate> campos)
        {
            // Verifica se o usuário tem permissão para criar um template
            if (!_permissaoUsuario.PodeCriarTemplate())  
            {
                throw new UnauthorizedAccessException("Você não tem permissão para criar o template.");
            }

            var template = new Template(nome, campos);
            return await _templateRepository.CriarAsync(template);
        }
    }
}