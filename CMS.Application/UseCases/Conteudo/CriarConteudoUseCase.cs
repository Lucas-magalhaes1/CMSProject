// CMS.Application/UseCases/Conteudos/CriarConteudoUseCase.cs
using CMS.Application.Interfaces;
using CMS.Domain.Entities;
using CMS.Domain.ValueObjects;

namespace CMS.Application.UseCases.Conteudos;

public class CriarConteudoUseCase
{
    private readonly ITemplateRepository _templateRepository;
    private readonly IConteudoRepository _conteudoRepository;

    public CriarConteudoUseCase(ITemplateRepository templateRepository, IConteudoRepository conteudoRepository)
    {
        _templateRepository = templateRepository;
        _conteudoRepository = conteudoRepository;
    }

    public async Task<Conteudo> ExecuteAsync(string titulo, Guid templateId, List<CampoPreenchido> camposPreenchidos)
    {
        // Obter template
        var template = await _templateRepository.ObterPorIdAsync(templateId);
    
        // Se o template não for encontrado, lançar erro
        if (template == null)
        {
            throw new ArgumentException($"Template com ID '{templateId}' não encontrado.");
        }

        // Validar se todos os campos obrigatórios do template foram preenchidos
        foreach (var campo in template.Campos.Where(c => c.Obrigatorio))
        {
            var campoPreenchido = camposPreenchidos.FirstOrDefault(c => c.Nome == campo.Nome);

            // Se não encontrar o campo preenchido ou o valor estiver vazio
            if (campoPreenchido == null || string.IsNullOrEmpty(campoPreenchido?.Valor))
            {
                throw new ArgumentException($"O campo '{campo.Nome}' é obrigatório e não foi preenchido. Favor preencher o campo antes de prosseguir.");
            }
        }

        // Criar conteúdo com os dados validados
        var conteudo = new Conteudo(titulo, template, camposPreenchidos)
        {
            Status = "Rascunho" // Status como "Rascunho"
        };

        // Tentar criar e retornar o conteúdo
        try
        {
            return await _conteudoRepository.CriarAsync(conteudo);
        }
        catch (Exception ex)
        {
            // Em caso de falha ao criar o conteúdo, lançar um erro mais informativo
            throw new InvalidOperationException("Erro ao tentar criar o conteúdo. Detalhes do erro: " + ex.Message, ex);
        }
    }
}