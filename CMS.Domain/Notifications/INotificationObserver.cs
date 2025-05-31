using CMS.Domain.Events;

public interface INotificationObserver
{
    void Update(ConteudoPublicadoEvent conteudoPublicadoEvent);
}