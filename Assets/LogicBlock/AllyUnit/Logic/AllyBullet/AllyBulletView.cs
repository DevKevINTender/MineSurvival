using System;
using UnityEngine;
using Zenject;

public class AllyBulletView : PoolingView
{

}

public class AllyBuletViewService : PoolingViewService
{
    private AllyBulletView _bulletView;
    private BulletComponent _bulletComponent;
    private AllyDealDamageComponent _allyDealDamageComponent;

    public void ActivateService(Vector3 bulletSpawnPos, Vector3 target, float damage)
    {
        _bulletView = (AllyBulletView) _poolingView;
        _bulletView.gameObject.SetActive(true);
        _bulletView.transform.position = bulletSpawnPos;

        _bulletComponent = _bulletView.GetComponent<BulletComponent>();
        _bulletComponent.ActivateComponent(target - bulletSpawnPos);

        _allyDealDamageComponent = _bulletView.GetComponent<AllyDealDamageComponent>();
        _allyDealDamageComponent.ActivateComponent(damage);
    }  
}

public class PoolingView : MonoBehaviour
{

}

public class PoolingViewService: IPoolingViewService
{
    [Inject] private IViewFabric _viewFabric;
    protected PoolingView _poolingView;
    private Action<IPoolingViewService> _deactivateAction;
    private Transform _poolTarget;

    public void ActivateServiceFromPool<T>(Transform poolTarget) where T : PoolingView
    {
        _poolTarget = poolTarget;
        _poolingView = _viewFabric.Init<T>(poolTarget);
        _poolingView.gameObject.SetActive(false);
    }

    public void SetDeactivateAction(Action<IPoolingViewService> action)
    {
        _deactivateAction = action;
    }

    public void DeactivateServiceToPool()
    {
        _poolingView?.gameObject.SetActive(false);
        _deactivateAction.Invoke(this);
    }

    public void ActivateServiceFromPool(Transform poolTarget)
    {

    }
}
