// CMS.Application/UseCases/Conteudos/EditarConteudoUseCase.cs
using CMS.Application.Interfaces;
using CMS.Domain.Entities;

namespace CMS.Application.UseCases.Conteudos;

public class EditarConteudoUseCase
{
    private readonly IConteudoRepository _conteudoRepository;

    public EditarConteudoUseCase(IConteudoRepository conteudoRepository)
    {
        _conteudoRepository = conteudoRepository;
    }

    public async Task<Conteudo?> ExecuteAsync(Guid id, List<CampoPreenchido> camposPreenchidos)  
    {
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