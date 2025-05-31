using CMS.Application.Interfaces;
using CMS.Domain.Entities;

namespace CMS.Application.UseCases.Conteudos
{
    public class ListarConteudosUseCase
    {
        private readonly IConteudoRepository _conteudoRepository;
        private readonly IPermissaoUsuario _permissaoUsuario;

        public ListarConteudosUseCase(IConteudoRepository conteudoRepository, IPermissaoUsuario permissaoUsuario)
        {
            _conteudoRepository = conteudoRepository;
            _permissaoUsuario = permissaoUsuario;
        }

        public async Task<List<Conteudo>> ExecuteAsync(Guid usuarioId)
        {
            
            if (_permissaoUsuario.PodeAprovarConteudo())  
            {
                return await _conteudoRepository.ListarAsync();
            }
            return await _conteudoRepository.ListarPorCriadorAsync(usuarioId); 
        }
    }
}