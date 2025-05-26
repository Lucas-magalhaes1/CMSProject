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
        // Obtém o conteúdo pelo ID
        var conteudo = await _conteudoRepository.ObterPorIdAsync(id);
        if (conteudo == null)
        {
            _logger.LogWarning($"Conteúdo com ID: {id} não encontrado.");
            return null;
        }

        _logger.LogInformation($"Conteúdo com ID: {id} carregado para edição.");

        // Verifica se o conteúdo é do usuário ou se ele tem permissão para editar
        if (conteudo.CriadoPor != usuarioId && !_permissaoUsuario.PodeEditarConteudo())
        {
            _logger.LogWarning($"Usuário com ID: {usuarioId} não tem permissão para editar o conteúdo com ID: {id}");
            throw new UnauthorizedAccessException("Você não tem permissão para editar este conteúdo.");
        }

        // Se o título foi enviado, altera o título
        if (!string.IsNullOrEmpty(titulo))
        {
            conteudo.AlterarTitulo(titulo);
        }

        // Atualiza ou adiciona os campos preenchidos
        foreach (var campo in camposPreenchidos)
        {
            var campoExistente = conteudo.CamposPreenchidos
                .FirstOrDefault(c => c.Nome == campo.Nome);

            if (campoExistente != null)
            {
                // Atualiza o valor do campo existente usando o método
                campoExistente.AtualizarValor(campo.Valor);
            }
            else
            {
                // Adiciona o campo novo
                conteudo.CamposPreenchidos.Add(campo);
            }
        }

        // Atualiza no repositório
        await _conteudoRepository.AtualizarAsync(conteudo);

        _logger.LogInformation($"Conteúdo com ID: {id} atualizado com sucesso.");
        return conteudo;
    }
}
