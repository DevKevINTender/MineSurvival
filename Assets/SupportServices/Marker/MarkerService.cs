using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using UniRx;
using System;

public class  MarkerService : IMarkerService
{
    private List<IMarker> markers = new List<IMarker>();

    private IEventService _eventService;
    private int count;

    private IDisposable eventSubscription;

    [Inject]
    public void Constructor
    (
        IEventService eventService
    )
    {
     
        _eventService = eventService;
    }

    public void ActivateService()
    {
        eventSubscription = _eventService.ObserveEvent<OnMarkerAwake>().Subscribe(SetMarker);
    }

    public void DeactivateService()
    {
        eventSubscription?.Dispose();
    }

    public Transform GetTransformMarker<T>() where T : MonoBehaviour, IMarker 
    {
        return markers.OfType<T>().FirstOrDefault().transform;
    }

    public void SetMarker(OnMarkerAwake markerAwake)
    {
        markers.Add(markerAwake.Marker);
    }
}

public interface IMarkerService: IService
{
    public Transform GetTransformMarker<T>() where T : MonoBehaviour, IMarker;
}

public class Marker: MonoBehaviour, IMarker
{
    private IEventService _eventService;

    public void Awake()
    {
        _eventService = SceneContainerAccessor.SceneContainer.Resolve<IEventService>();
        _eventService.PublishEvent(new OnMarkerAwake { Marker = this });
    }
}

public interface IMarker
{
    
}
