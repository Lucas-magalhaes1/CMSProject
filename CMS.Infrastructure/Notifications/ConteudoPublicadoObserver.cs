using CMS.Application.Interfaces;
using CMS.Domain.Events;
using CMS.Domain.Entities;


namespace CMS.Infrastructure.Notifications
{
    public class ConteudoPublicadoObserver : INotificationObserver
    {
        private readonly INotificacaoRepository _notificacaoRepository;

        public ConteudoPublicadoObserver(INotificacaoRepository notificacaoRepository)
        {
            _notificacaoRepository = notificacaoRepository;
        }

        public async void Update(ConteudoPublicadoEvent conteudoPublicadoEvent)
        {
            var mensagem = $"Seu conteúdo '{conteudoPublicadoEvent.Titulo}' foi aprovado em {conteudoPublicadoEvent.DataPublicacao}.";

            var notificacao = new Notificacao(
                usuarioId: conteudoPublicadoEvent.CriadorId,
                titulo: "Conteúdo Aprovado",
                mensagem: mensagem);

            await _notificacaoRepository.AdicionarAsync(notificacao);
        }
    }
}