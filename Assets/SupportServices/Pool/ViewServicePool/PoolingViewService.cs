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
    [Inject] private IViewPoolService _viewPoolService;
    private Action<IPoolingViewService> _deactivateAction;


    public void ActivateServiceFromPool(Action<IPoolingViewService> action)
    {
        _deactivateAction = action;
        viewPool = _viewPoolService.GetPoolTransfrom();
    }

    public void DeactivateServiceToPool()
    {
        _deactivateAction.Invoke(this);
    }
}
