using CMS.Application.Interfaces;
using CMS.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificacoesController : ControllerBase
{
    private readonly INotificacaoRepository _notificacaoRepository;

    public NotificacoesController(INotificacaoRepository notificacaoRepository)
    {
        _notificacaoRepository = notificacaoRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetNotificacoes()
    {
        var usuarioIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(usuarioIdString, out var usuarioId))
        {
            return Unauthorized();
        }

        var notificacoes = await _notificacaoRepository.ObterPorUsuarioIdAsync(usuarioId);
        return Ok(notificacoes);
    }

    [HttpPost("{id}/marcar-como-lida")]
    public async Task<IActionResult> MarcarComoLida(Guid id)
    {
        var notificacao = await _notificacaoRepository.ObterPorIdAsync(id);
        if (notificacao == null)
            return NotFound();

        notificacao.MarcarComoLida();
        await _notificacaoRepository.AtualizarAsync(notificacao);

        return NoContent();
    }
}