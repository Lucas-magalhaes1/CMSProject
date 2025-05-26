using System.Security.Claims;
using CMS.Application.DTOs;
using CMS.Application.Interfaces;
using CMS.Application.UseCases.Conteudos;
using CMS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ConteudosController : ControllerBase
{
    private readonly CriarConteudoUseCase _criarConteudoUseCase;
    private readonly ListarConteudosUseCase _listarConteudosUseCase;
    private readonly ObterConteudoPorIdUseCase _obterConteudoPorIdUseCase;
    private readonly EditarConteudoUseCase _editarConteudoUseCase;
    private readonly ClonarConteudoUseCase _clonarConteudoUseCase;
    private readonly DeletarConteudoUseCase _deletarConteudoUseCase;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPermissaoUsuario _permissaoUsuario;
    private readonly ILogger<ConteudosController> _logger;
    private readonly ITemplateRepository _templateRepository;  

    public ConteudosController(
        CriarConteudoUseCase criarConteudoUseCase,
        ListarConteudosUseCase listarConteudosUseCase,
        ObterConteudoPorIdUseCase obterConteudoPorIdUseCase,
        EditarConteudoUseCase editarConteudoUseCase,
        ClonarConteudoUseCase clonarConteudoUseCase,
        DeletarConteudoUseCase deletarConteudoUseCase,
        IUsuarioRepository usuarioRepository,
        IPermissaoUsuario permissaoUsuario,
        ILogger<ConteudosController> logger,
        ITemplateRepository templateRepository)
    {
        _criarConteudoUseCase = criarConteudoUseCase;
        _listarConteudosUseCase = listarConteudosUseCase;
        _obterConteudoPorIdUseCase = obterConteudoPorIdUseCase;
        _editarConteudoUseCase = editarConteudoUseCase;
        _clonarConteudoUseCase = clonarConteudoUseCase;
        _deletarConteudoUseCase = deletarConteudoUseCase;
        _usuarioRepository = usuarioRepository;
        _permissaoUsuario = permissaoUsuario;
        _logger = logger;
        _templateRepository = templateRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] ConteudoDto conteudoDto)
    {
        try
        {
            var usuarioId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId);
            if (usuario == null)
            {
                return BadRequest(ResponseDto<string>.Falha("Usuário não encontrado"));
            }

            conteudoDto.NomeCriador = usuario.Nome;

            var camposPreenchidos = conteudoDto.CamposPreenchidos
                .Select(c => new CampoPreenchido(c.Nome, c.Valor))
                .ToList();

            var conteudo = await _criarConteudoUseCase.ExecuteAsync(conteudoDto.Titulo, conteudoDto.TemplateId, camposPreenchidos, usuarioId,usuario.Nome);

            var responseDto = new ConteudoDto
            {
                Id = conteudo.Id,
                Titulo = conteudo.Titulo,
                TemplateId = conteudo.Template.Id,
                Status = conteudo.Status,
                CamposPreenchidos = conteudo.CamposPreenchidos.Select(c => new CampoConteudoDto
                {
                    Nome = c.Nome,
                    Valor = c.Valor
                }).ToList(),
                Comentario = conteudo.Comentario,
                NomeCriador = conteudoDto.NomeCriador,
            };

            return Ok(ResponseDto<ConteudoDto>.Ok(responseDto, "Conteúdo criado com sucesso"));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ResponseDto<string>.Falha($"Erro: {ex.Message}"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<string>.Falha($"Erro interno: {ex.Message}"));
        }
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        try
        {
            var usuarioId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var conteudos = await _listarConteudosUseCase.ExecuteAsync(usuarioId);

            var responseDtos = conteudos.Select(c => new ConteudoDto
            {
                Id = c.Id,
                Titulo = c.Titulo,
                TemplateId = c.Template.Id,
                Status = c.Status,
                CamposPreenchidos = c.CamposPreenchidos.Select(campo => new CampoConteudoDto
                {
                    Nome = campo.Nome,
                    Valor = campo.Valor
                }).ToList(),
                NomeCriador = c.NomeCriador,
                Comentario = c.Comentario
            }).ToList();

            return Ok(ResponseDto<List<ConteudoDto>>.Ok(responseDtos));
        }
        catch (UnauthorizedAccessException ex)
        {
            // Retorna uma resposta 403 Forbidden com a mensagem de erro
            return Forbid(); // Ou, para incluir a mensagem, use o StatusCode diretamente
        }

        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<string>.Falha($"Erro interno: {ex.Message}"));
        }
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Editar(Guid id, [FromBody] ConteudoDto conteudoDto)
    {
        try
        {
            var usuarioId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            // Carrega o template associado ao conteúdo para validação
            var template = await _templateRepository.ObterPorIdAsync(conteudoDto.TemplateId);
            if (template == null)
            {
                return BadRequest(ResponseDto<string>.Falha("Template não encontrado."));
            }

            // Recarrega os campos preenchidos do DTO
            var camposPreenchidos = conteudoDto.CamposPreenchidos
                .Select(c => new CampoPreenchido(c.Nome, c.Valor))
                .ToList();

            // Chama o UseCase para editar o conteúdo e passar o título
            var conteudo = await _editarConteudoUseCase.ExecuteAsync(id, camposPreenchidos, usuarioId, conteudoDto.Titulo);

            if (conteudo == null)
            {
                return NotFound(ResponseDto<string>.Falha("Conteúdo não encontrado"));
            }

            // Prepara o DTO de resposta com os dados atualizados
            var responseDto = new ConteudoDto
            {
                Id = conteudo.Id,
                Titulo = conteudo.Titulo,
                TemplateId = conteudo.Template?.Id ?? Guid.Empty,
                Status = conteudo.Status,
                CamposPreenchidos = conteudo.CamposPreenchidos.Select(c => new CampoConteudoDto
                {
                    Nome = c.Nome,
                    Valor = c.Valor
                }).ToList(),
                Comentario = conteudo.Comentario
            };

            return Ok(ResponseDto<ConteudoDto>.Ok(responseDto, "Conteúdo editado com sucesso"));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<string>.Falha($"Erro interno: {ex.Message}"));
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        try
        {
            var usuarioId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            // Verificar permissão de acesso ao conteúdo por ID
            if (!_permissaoUsuario.PodeObterConteudoPorId())
                return Forbid();

            var conteudo = await _obterConteudoPorIdUseCase.ExecuteAsync(id);

            if (conteudo == null)
                return NotFound(ResponseDto<string>.Falha("Conteúdo não encontrado"));

            // Permitir acesso se for admin/editor ou criador do conteúdo
            bool usuarioEhAdminOuEditor = _permissaoUsuario.PodeAprovarConteudo(); // Considera admin/editor
            if (!usuarioEhAdminOuEditor && conteudo.CriadoPor != usuarioId)
                return Forbid();

            var responseDto = new ConteudoDto
            {
                Id = conteudo.Id,
                Titulo = conteudo.Titulo,
                TemplateId = conteudo.Template.Id,
                Status = conteudo.Status,
                CamposPreenchidos = conteudo.CamposPreenchidos.Select(c => new CampoConteudoDto
                {
                    Nome = c.Nome,
                    Valor = c.Valor
                }).ToList(),
                NomeCriador = conteudo.NomeCriador,
                Comentario = conteudo.Comentario
            };

            return Ok(ResponseDto<ConteudoDto>.Ok(responseDto));
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<string>.Falha($"Erro interno: {ex.Message}"));
        }
    }

    [HttpPost("{id}/clone")]
    public async Task<IActionResult> Clonar(Guid id)
    {
        try
        {
            var usuarioId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var conteudoClonado = await _clonarConteudoUseCase.ExecuteAsync(id, usuarioId);

            if (conteudoClonado == null)
                return NotFound(ResponseDto<string>.Falha("Conteúdo original não encontrado"));

            var responseDto = new ConteudoDto
            {
                Id = conteudoClonado.Id,
                Titulo = conteudoClonado.Titulo,
                TemplateId = conteudoClonado.Template?.Id ?? Guid.Empty,
                Status = conteudoClonado.Status,
                CamposPreenchidos = conteudoClonado.CamposPreenchidos.Select(c => new CampoConteudoDto
                {
                    Nome = c.Nome,
                    Valor = c.Valor
                }).ToList(),
                Comentario = conteudoClonado.Comentario
            };

            return Ok(ResponseDto<ConteudoDto>.Ok(responseDto, "Conteúdo clonado com sucesso"));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<string>.Falha($"Erro interno: {ex.Message}"));
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Deletar(Guid id)
    {
        try
        {
            var usuarioId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            bool deletado = await _deletarConteudoUseCase.ExecuteAsync(id, usuarioId);

            if (!deletado)
                return NotFound(ResponseDto<string>.Falha("Conteúdo não encontrado"));

            return Ok(ResponseDto<string>.Ok(null, "Conteúdo deletado com sucesso"));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<string>.Falha($"Erro interno: {ex.Message}"));
        }
    }
}
