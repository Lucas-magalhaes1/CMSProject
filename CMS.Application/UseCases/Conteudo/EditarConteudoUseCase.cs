using CMS.Application.Interfaces;
using CMS.Domain.Entities;
using Microsoft.Extensions.Logging;

public class EditarConteudoUseCase
{
    private readonly IConteudoRepository _conteudoRepository;
    private readonly IPermissaoUsuario _permissaoUsuario;
    private readonly ILogger<EditarConteudoUseCase> _logger;

    public EditarConteudoUseCase(IConteudoRepository conteudoRepository, IPermissaoUsuario permissaoUsuario, ILogger<EditarConteudoUseCase> logger)
    {
        _conteudoRepository = conteudoRepository;
        _permissaoUsuario = permissaoUsuario;
        _logger = logger;
    }

    public async Task<Conteudo?> ExecuteAsync(Guid id, List<CampoPreenchido> camposPreenchidos, Guid usuarioId, string? titulo)
    {
        
        var conteudo = await _conteudoRepository.ObterPorIdAsync(id);
        if (conteudo == null)
        {
            _logger.LogWarning($"Conteúdo com ID: {id} não encontrado.");
            return null;
        }

        _logger.LogInformation($"Conteúdo com ID: {id} carregado para edição.");

      
        if (conteudo.CriadoPor != usuarioId && !_permissaoUsuario.PodeEditarConteudo())
        {
            _logger.LogWarning($"Usuário com ID: {usuarioId} não tem permissão para editar o conteúdo com ID: {id}");
            throw new UnauthorizedAccessException("Você não tem permissão para editar este conteúdo.");
        }

        
        if (!string.IsNullOrEmpty(titulo))
        {
            conteudo.AlterarTitulo(titulo);
        }

        
        foreach (var campo in camposPreenchidos)
        {
            var campoExistente = conteudo.CamposPreenchidos
                .FirstOrDefault(c => c.Nome == campo.Nome);

            if (campoExistente != null)
            {
                campoExistente.AtualizarValor(campo.Valor);
            }
            else
            {
                conteudo.CamposPreenchidos.Add(campo);
            }
        }
        
        await _conteudoRepository.AtualizarAsync(conteudo);

        _logger.LogInformation($"Conteúdo com ID: {id} atualizado com sucesso.");
        return conteudo;
    }
}
