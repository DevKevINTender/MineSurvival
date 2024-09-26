using System;
using System.Collections;
using UniRx;
using UnityEngine;
using Zenject;

public class MeleeProjectileView : MonoBehaviour
{
    public Action OnDestroyAction;
    public void ActivateView(Vector3 spawnPos)
    {
        transform.parent = null;
        gameObject.SetActive(true);
        transform.position = spawnPos;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        OnDestroyAction?.Invoke();
    }

    public void DeactivateView(Transform parent)
    {
        transform.parent = parent;
        transform.localPosition = Vector3.zero;
        gameObject.SetActive(false);
    }
}

public class MeleeProjectileViewService : PoolingViewService
{
    [Inject] private IViewFabric _viewFabric;
    private MeleeProjectileView _view;
    private DealDamageComponent _allyDealDamageComponent;
    private CompositeDisposable _disposables = new();
    public void ActivateService(Vector3 projectileSpawnPos, float damage, DealDamageType type, MeleeProjectileView meleeProjectilePrefab)
    {
        _view = _view != null ? _view : _viewFabric.Init(meleeProjectilePrefab);
        _view.ActivateView(projectileSpawnPos);

        _allyDealDamageComponent = _view.GetComponent<DealDamageComponent>();
        _allyDealDamageComponent.ActivateComponent(damage, type);

        _view.OnDestroyAction += DeactivateService;
            
    }


    public void DeactivateService()
    {
        _view.DeactivateView(viewPool);
        _view.OnDestroyAction -= DeactivateService;
        DeactivateServiceToPool();
    }
}

