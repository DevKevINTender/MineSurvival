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
        StartCoroutine(DestroyDelay());
    }

    private IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(0.1f);
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
    private AllyDealDamageComponent _allyDealDamageComponent;
    private CompositeDisposable _disposables = new();
    public void ActivateService(Vector3 bulletSpawnPos, float damage)
    {
        _view = _view != null ? _view : _viewFabric.Init<MeleeProjectileView>();
        _view.ActivateView(bulletSpawnPos);

        _allyDealDamageComponent = _view.GetComponent<AllyDealDamageComponent>();
        _allyDealDamageComponent.ActivateComponent(damage);

        _view.OnDestroyAction += DeactivateService;
            
    }


    public void DeactivateService()
    {
        _view.DeactivateView(viewPool);
        _view.OnDestroyAction -= DeactivateService;
        DeactivateServiceToPool();
    }
}

