using CMS.Application.Interfaces;
using CMS.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ListarConteudosAprovadosUseCase
{
    private readonly IConteudoRepository _conteudoRepository;

    public ListarConteudosAprovadosUseCase(IConteudoRepository conteudoRepository)
    {
        _conteudoRepository = conteudoRepository;
    }

    public async Task<List<Conteudo>> ExecuteAsync()
    {
        return await _conteudoRepository.ListarPorStatusAsync("Aprovado");
    }
}