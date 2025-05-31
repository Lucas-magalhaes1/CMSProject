using CMS.Application.Interfaces;
using CMS.Domain.Chain.Handlers;
using CMS.Domain.Entities;
using CMS.Domain.Events;
using System;
using System.Threading.Tasks;

public class AprovarConteudoUseCase
{
    private readonly IConteudoRepository _conteudoRepository;
    private readonly AprovarConteudoHandler _aprovarConteudoHandler;
    private readonly IPermissaoUsuario _permissaoUsuario;
    private readonly NotificationPublisher _notificationPublisher;
    private readonly INotificationObserver _conteudoPublicadoObserver;

    public AprovarConteudoUseCase(
        IConteudoRepository conteudoRepository,
        AprovarConteudoHandler aprovarConteudoHandler,
        IPermissaoUsuario permissaoUsuario,
        NotificationPublisher notificationPublisher,
        INotificationObserver conteudoPublicadoObserver) 
    {
        _conteudoRepository = conteudoRepository;
        _aprovarConteudoHandler = aprovarConteudoHandler;
        _permissaoUsuario = permissaoUsuario;
        _notificationPublisher = notificationPublisher;
        _conteudoPublicadoObserver = conteudoPublicadoObserver;

        
        _notificationPublisher.Register(_conteudoPublicadoObserver);
    }

    public async Task<Conteudo?> ExecuteAsync(Guid id)
    {
        if (!_permissaoUsuario.PodeAprovarConteudo())
            throw new UnauthorizedAccessException("Você não tem permissão para aprovar conteúdo.");

        var conteudo = await _conteudoRepository.ObterPorIdAsync(id);
        if (conteudo == null)
            return null;

        var conteudoAprovado = await _aprovarConteudoHandler.ManipularConteudo(conteudo);
        await _conteudoRepository.AtualizarAsync(conteudoAprovado);

        
        var conteudoPublicadoEvent = new ConteudoPublicadoEvent(
            conteudoAprovado.Id,
            conteudoAprovado.Titulo,
            conteudoAprovado.CriadoPor,
            conteudoAprovado.NomeCriador,
            DateTime.Now);

        
        _notificationPublisher.Notify(conteudoPublicadoEvent);

        return conteudoAprovado;
    }
}
