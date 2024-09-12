using System;
using UniRx;

public interface IEventService
{
    IObservable<T> ObserveEvent<T>();
    void PublishEvent<T>(T eventData);
}

public class EventService : IEventService
{
    private Subject<object> eventSubject = new Subject<object>();

    public void PublishEvent<T>(T eventData)
    {
        eventSubject.OnNext(eventData);
    }

    public IObservable<T> ObserveEvent<T>()
    {
        return eventSubject.Where(data => data is T).Select(data => (T)data).AsObservable();
    }
}
