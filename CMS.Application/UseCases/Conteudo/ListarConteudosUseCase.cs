using CMS.Application.Interfaces;
using CMS.Domain.Entities;

namespace CMS.Application.UseCases.Conteudos;

public class ListarConteudosUseCase
{
    private readonly IConteudoRepository _conteudoRepository;

    public ListarConteudosUseCase(IConteudoRepository conteudoRepository)
    {
        _conteudoRepository = conteudoRepository;
    }

    public async Task<List<Conteudo>> ExecuteAsync()
    {
        return await _conteudoRepository.ListarAsync();
    }
}