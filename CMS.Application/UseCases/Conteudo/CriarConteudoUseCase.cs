using CMS.Application.Interfaces;
using CMS.Domain.Entities;

public class CriarConteudoUseCase
{
    private readonly ITemplateRepository _templateRepository;
    private readonly IConteudoRepository _conteudoRepository;
    private readonly IPermissaoUsuario _permissaoUsuario;
    
    public CriarConteudoUseCase(ITemplateRepository templateRepository, IConteudoRepository conteudoRepository, IPermissaoUsuario permissaoUsuario)
    {
        _templateRepository = templateRepository;
        _conteudoRepository = conteudoRepository;
        _permissaoUsuario = permissaoUsuario;
    }

    public async Task<Conteudo> ExecuteAsync(string titulo, Guid templateId, List<CampoPreenchido> camposPreenchidos, Guid usuarioId, string nomeCriador)
    {
        if (!_permissaoUsuario.PodeCriarConteudo())
            throw new UnauthorizedAccessException("Você não tem permissão para criar conteúdo.");

        var template = await _templateRepository.ObterPorIdAsync(templateId);
        if (template == null)
            throw new ArgumentException($"Template com ID '{templateId}' não encontrado.");

        foreach (var campo in template.Campos.Where(c => c.Obrigatorio))
        {
            var campoPreenchido = camposPreenchidos.FirstOrDefault(c => c.Nome == campo.Nome);
            if (campoPreenchido == null || string.IsNullOrEmpty(campoPreenchido.Valor))
                throw new ArgumentException($"O campo '{campo.Nome}' é obrigatório e não foi preenchido.");
        }

        var conteudo = new Conteudo(titulo, template, camposPreenchidos, usuarioId, nomeCriador)
        {
            Status = "Rascunho"
        };

        try
        {
            return await _conteudoRepository.CriarAsync(conteudo);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Erro ao tentar criar o conteúdo. Detalhes do erro: " + ex.Message, ex);
        }
    }
}
