using CMS.Application.Interfaces;
using CMS.Domain.Entities;
using CMS.Domain.Events;
using Microsoft.Extensions.DependencyInjection;

namespace CMS.Infrastructure.Notifications;

public class ConteudoPublicadoObserver : INotificationObserver
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ConteudoPublicadoObserver(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

   
    public void Update(ConteudoPublicadoEvent conteudoPublicadoEvent)
    {
        
        _ = UpdateAsync(conteudoPublicadoEvent);
    }

   
    public async Task UpdateAsync(ConteudoPublicadoEvent conteudoPublicadoEvent)
    {
        using var scope = _scopeFactory.CreateScope();
        var notificacaoRepository = scope.ServiceProvider.GetRequiredService<INotificacaoRepository>();

        var mensagem = $"Seu conteúdo '{conteudoPublicadoEvent.Titulo}' foi aprovado em {conteudoPublicadoEvent.DataPublicacao}.";

        var notificacao = new Notificacao(
            usuarioId: conteudoPublicadoEvent.CriadorId,
            titulo: "Conteúdo Aprovado",
            mensagem: mensagem);

        await notificacaoRepository.AdicionarAsync(notificacao);
    }
}