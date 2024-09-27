using System;
using UniRx;
using UnityEngine;
using Zenject;

public class HealProjectileView : MonoBehaviour
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

public class HealProjectileViewService : PoolingViewService
{
    [Inject] private IViewFabric _viewFabric;
    private HealProjectileView _view;
    private DealHealComponent _dealHealComponent;
    private CompositeDisposable _disposables = new();
    public void ActivateService(Vector3 projectileSpawnPos, float damage, HealProjectileView meleeProjectilePrefab)
    {
        _view = _view != null ? _view : _viewFabric.Init(meleeProjectilePrefab);
        _view.ActivateView(projectileSpawnPos);

        _dealHealComponent = _view.GetComponent<DealHealComponent>();
        _dealHealComponent.ActivateComponent(damage, DealHealType.Ally);

        _view.OnDestroyAction += DeactivateService;

    }


    public void DeactivateService()
    {
        _view.DeactivateView(viewPool);
        _view.OnDestroyAction -= DeactivateService;
        DeactivateServiceToPool();
    }
}



