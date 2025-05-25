using CMS.Application.Interfaces;
using CMS.Domain.Entities;

namespace CMS.Application.UseCases.Templates
{
    public class ListarTemplatesUseCase
    {
        private readonly ITemplateRepository _templateRepository;
        private readonly IPermissaoUsuario _permissaoUsuario;

        public ListarTemplatesUseCase(ITemplateRepository templateRepository, IPermissaoUsuario permissaoUsuario)
        {
            _templateRepository = templateRepository;
            _permissaoUsuario = permissaoUsuario;
        }

        public async Task<List<Template>> ExecuteAsync()
        {
            // Verifica se o usuário tem permissão para listar templates
            if (!_permissaoUsuario.PodeListarTemplates())  
            {
                throw new UnauthorizedAccessException("Você não tem permissão para listar os templates.");
            }

            return await _templateRepository.ListarAsync();
        }
    }
}