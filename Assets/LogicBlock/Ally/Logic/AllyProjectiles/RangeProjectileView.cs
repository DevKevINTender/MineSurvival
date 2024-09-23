using UnityEngine;
using Zenject;

public class RangeProjectileView : MonoBehaviour
{
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
    private RangeProjectileView _bulletView;
    private BulletComponent _bulletComponent;
    private DealDamageComponent _allyDealDamageComponent;
    private OnContactComponent _onContactComponent;

    public void ActivateService(Vector3 bulletSpawnPos, Vector3 target, float damage)
    {
        _bulletView ??= _viewFabric.Init<RangeProjectileView>();
        _bulletView.ActivateView(bulletSpawnPos);

        _bulletComponent = _bulletView.GetComponent<BulletComponent>();
        _bulletComponent.ActivateComponent(Vector3.Normalize(target - bulletSpawnPos) * 10);

        _allyDealDamageComponent = _bulletView.GetComponent<DealDamageComponent>();
        _allyDealDamageComponent.ActivateComponent(damage, DealDamageEnum.Ally);

        _onContactComponent = _bulletView.GetComponent<OnContactComponent>();
        _onContactComponent.Add(typeof(EnemyUnitView));
        _onContactComponent.Add(typeof(DestroyComponent));
        _onContactComponent.hasContactAction += DestroyBulletAction;
    }

    public void DestroyBulletAction()
    {
        DeactivateService();
    }

    public void DeactivateService()
    {
        _onContactComponent.hasContactAction -= DestroyBulletAction;
        _onContactComponent.DeactivateComponent();
        _bulletView.DeactivateView(viewPool);
        DeactivateServiceToPool();
    }
}