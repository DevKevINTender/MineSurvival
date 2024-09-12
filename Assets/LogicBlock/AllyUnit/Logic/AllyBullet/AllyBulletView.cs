using UnityEngine;

public class AllyBulletView : PoolingView
{

}

public class AllyBuletViewService : PoolingViewService
{
    private AllyBulletView _bulletView;
    private BulletComponent _bulletComponent;
    private AllyDealDamageComponent _allyDealDamageComponent;
    private OnContactComponent _onContactComponent;

    public void ActivateService(Vector3 bulletSpawnPos, Vector3 target, float damage)
    {
        _bulletView = ActivatePollingView<AllyBulletView>();
        _bulletView.gameObject.SetActive(true);
        _bulletView.transform.position = bulletSpawnPos;

        _bulletComponent = _bulletView.GetComponent<BulletComponent>();
        _bulletComponent.ActivateComponent(target - bulletSpawnPos);

        _allyDealDamageComponent = _bulletView.GetComponent<AllyDealDamageComponent>();
        _allyDealDamageComponent.ActivateComponent(damage);

        _onContactComponent = _bulletView.GetComponent<OnContactComponent>();
        _onContactComponent.Add(typeof(EnemyView));
        _onContactComponent.Add(typeof(DestroyComponent));
        _onContactComponent.hasContactAction += DestroyBulletAction;
    } 
    
    public void DestroyBulletAction()
    {
        _onContactComponent.hasContactAction -= DestroyBulletAction;
        DeactivateServiceToPool();
    }
}


