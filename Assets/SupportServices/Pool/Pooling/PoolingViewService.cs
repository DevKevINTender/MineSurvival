using System;
using UnityEngine;
using Zenject;

public interface IPoolingViewService
{
    public void ActivateServiceFromPool(Action<IPoolingViewService> action);
    public void DeactivateServiceToPool();
}
public class PoolingViewService: IPoolingViewService
{
    protected Transform viewPool;   
    [Inject] private IMarkerService _markerService;
    private Action<IPoolingViewService> _deactivateAction;

    public void ActivateServiceFromPool(Action<IPoolingViewService> action)
    {
        _deactivateAction = action;
        viewPool = _markerService.GetTransformMarker<ViewPoolMarker>();
    }

    public void DeactivateServiceToPool()
    {
        _deactivateAction.Invoke(this);
    }
}
