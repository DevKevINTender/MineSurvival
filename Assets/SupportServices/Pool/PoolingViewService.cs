using System;
using UnityEngine;
using Zenject;

public class PoolingViewService: IPoolingViewService
{
    [Inject] private IViewFabric _viewFabric;
    protected PoolingView _poolingView;
    private Action<IPoolingViewService> _deactivateAction;
    private Transform _poolTarget;

    public void ActivateServiceFromPool(Action<IPoolingViewService> action, Transform poolTarget)
    {
        _poolTarget = poolTarget;
        _deactivateAction = action;
    }

    public void DeactivateServiceToPool()
    {
        if(_poolingView is not null)
        {
            _poolingView.gameObject.SetActive(false);
            _poolingView.transform.SetParent(_poolTarget);
            _poolingView.transform.localPosition = Vector3.zero;
        }
        _deactivateAction.Invoke(this);
    }

    public T ActivatePollingView<T>() where T : PoolingView
    {
        _poolingView = _poolingView ?? _viewFabric.Init<T>(_poolTarget);
        return (T)_poolingView;
    }
}
