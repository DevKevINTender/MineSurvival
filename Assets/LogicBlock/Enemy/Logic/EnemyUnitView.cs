using Zenject;
using UnityEngine;
using System;

public class EnemyUnitView : PoolingView
{
    private Rigidbody2D rb;

    public void ActivateView(Transform spawnPos)
    {
        gameObject.SetActive(true);
        transform.parent = null;
        transform.localPosition = spawnPos.position;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector3.right;
    }

    public void DiactivateView(Transform parent)
    {
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        gameObject.SetActive(false);
    }
}

public class EnemyUnitViewService : PoolingViewService
{
	[Inject] private IViewFabric _fabric;
    [Inject] private IMarkerService _markerService;
    [Inject] private IViewPoolService _viewPoolService;
    [Inject] private EnemyListDataManager _enemyListDataManager;

    private EnemyData _enemyData;

    private EnemyUnitView _enemyView;
    private HpComponent _hpComponent;
    private TakeDamageComponent _takeDamageComponent;
    private Transform _spawnPos;


    public void ActivateService(EnemyEnum enemyName)
    {
        _enemyData = _enemyListDataManager.GetEnemyDataByName(enemyName);

        _spawnPos = _markerService.GetTransformMarker<EnemySpawnPosMarker>();
        _enemyView ??= _fabric.Init<EnemyUnitView>(_enemyData.prefab);
        _enemyView.ActivateView(_spawnPos);

        _hpComponent = _enemyView.GetComponent<HpComponent>();
        _hpComponent.ActivateComponent(100);
        _hpComponent.DieAction += OnDieAction;

        _takeDamageComponent = _enemyView.GetComponent<TakeDamageComponent>();
        _takeDamageComponent.ActivateComponent<AllyDealDamageComponent>();
    }
    
    public void DeactivateService()
    {
        Transform pool = _markerService.GetTransformMarker<ViewPoolMarker>();
        _enemyView.DiactivateView(pool);
        _hpComponent.DieAction -= OnDieAction;
        DeactivateServiceToPool();
    }

    public void OnDieAction()
    {
        DeactivateService();
    }
}



