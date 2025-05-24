using CMS.Application.Interfaces;
using CMS.Domain.Entities;

namespace CMS.Application.UseCases.Conteudos;

public class ObterConteudoPorIdUseCase
{
    private readonly IConteudoRepository _conteudoRepository;

    public ObterConteudoPorIdUseCase(IConteudoRepository conteudoRepository)
    {
        _conteudoRepository = conteudoRepository;
    }

    public async Task<Conteudo?> ExecuteAsync(Guid id)
    {
        return await _conteudoRepository.ObterPorIdAsync(id);
    }
}