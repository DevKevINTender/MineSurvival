using System;
using UnityEngine;
using Zenject;

public class RangeProjectileView : MonoBehaviour
{
    public MoveComponent MoveComponent;
    public DealDamageComponent DealDamageComponent;
    public OnContactComponent OnContactComponent;
    public void ActivateView(Vector3 bulletSpawnPos)
    {
        gameObject.SetActive(true);
        transform.position = bulletSpawnPos;
    }
    public void DeactivateView(Transform parent)
    {
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        gameObject.SetActive(false);
    }
}

public class RangeProjectileViewService : PoolingViewService
{
    [Inject] private IViewFabric _viewFabric;
    private RangeProjectileView _projectileView;

    public void ActivateService(Vector3 projectileSpawnPos, Transform target, float damage, RangeProjectileView projectilePrefab, DealDamageType dealDamageType)
    {
        _projectileView ??= _viewFabric.Init(projectilePrefab);
        _projectileView.ActivateView(projectileSpawnPos);

        _projectileView.MoveComponent.MoveToTarget(target, 10f, 0);


        _projectileView.DealDamageComponent.ActivateComponent(damage, dealDamageType);

        _projectileView.OnContactComponent.Add(typeof(EnemyUnitView));
        _projectileView.OnContactComponent.Add(typeof(DestroyComponent));
        _projectileView.OnContactComponent.hasContactAction += DeactivateService;
    }

    public void DestroyBulletAction()
    {
        DeactivateService();
    }

    public void DeactivateService()
    {
        _projectileView.OnContactComponent.hasContactAction -= DestroyBulletAction;
        _projectileView.OnContactComponent.DeactivateComponent();
        _projectileView.DealDamageComponent.DeactivateComponent();
        _projectileView.MoveComponent.DeactivateComponent();
        _projectileView.DeactivateView(viewPool);
        DeactivateServiceToPool();
    }
}