using CMS.Domain.Events;
using System.Collections.Generic;

public class NotificationPublisher
{
    private readonly List<INotificationObserver> _observers = new();

    public void Register(INotificationObserver observer)
    {
        if (!_observers.Contains(observer))
            _observers.Add(observer);
    }

    public void Unregister(INotificationObserver observer)
    {
        _observers.Remove(observer);
    }

    public void Notify(ConteudoPublicadoEvent conteudoPublicadoEvent)
    {
        foreach (var observer in _observers)
        {
            observer.Update(conteudoPublicadoEvent);
        }
    }
}