using CMS.Application.Interfaces;
using CMS.Domain.Entities;
using System;

namespace CMS.Application.UseCases.Templates
{
    public class ObterTemplatePorIdUseCase
    {
        private readonly ITemplateRepository _templateRepository;
        private readonly IPermissaoUsuario _permissaoUsuario;

        public ObterTemplatePorIdUseCase(ITemplateRepository templateRepository, IPermissaoUsuario permissaoUsuario)
        {
            _templateRepository = templateRepository;
            _permissaoUsuario = permissaoUsuario;
        }

        public async Task<Template?> ExecuteAsync(Guid id)
        {
            
            if (!_permissaoUsuario.PodeObterTemplatePorId())  
            {
                throw new UnauthorizedAccessException("Você não tem permissão para visualizar o template.");
            }

            return await _templateRepository.ObterPorIdAsync(id);
        }
    }
}