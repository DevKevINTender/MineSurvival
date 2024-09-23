using Zenject;
using UnityEngine;
using UniRx;
using UnityEditor;

public class EnemyUnitView: MonoBehaviour
{
    public HpComponent hpComponent;
    public MoveComponent moveComponent;
    public DealDamageComponent dealDamageComponent;
    public TakeDamageComponent takeDamageComponent;
    public TargetFinderComponent targetFinderComponentForMovement;
    public TargetFinderComponent targetFinderComponentFoAttack;

    public void ActivateView(Transform spawnPos)
    {
        gameObject.SetActive(true);
        transform.parent = null;
        transform.localPosition = spawnPos.position;
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
	[Inject] private IViewFabric _viewFabric;
	[Inject] private IServiceFabric _serviceFabric;
    [Inject] private IMarkerService _markerService;
    [Inject] private IViewPoolService _viewPoolService;
    [Inject] private EnemyListDataManager _enemyListDataManager;

    private EnemyData _enemyData;
    private EnemyUnitView _enemyView;
    private Transform _spawnPos;

    private MeleeAtackService _meleeAtackService;
    private MoveService _moveService;

    private HpComponent _hpComponent;
    private MoveComponent _moveComponent;
    private TakeDamageComponent _takeDamageComponent;
    private DealDamageComponent _dealDamageComponent;
    private TargetFinderComponent _targetFinderComponentForMovement;
    private TargetFinderComponent _targetFinderComponentFoAttack;

    private CompositeDisposable _disposables = new();

    public void ActivateService(EnemyEnum enemyName)
    {
        _enemyData = _enemyListDataManager.GetEnemyDataByName(enemyName);

        _spawnPos = _markerService.GetTransformMarker<EnemySpawnPosMarker>();
        _enemyView ??= _viewFabric.Init<EnemyUnitView>(_enemyData.prefab);
        _enemyView.ActivateView(_spawnPos);

        _hpComponent = _enemyView.hpComponent;
        _hpComponent.ActivateComponent(1000);
        _hpComponent.DieAction += DeactivateService;

        _takeDamageComponent = _enemyView.takeDamageComponent;
        _takeDamageComponent.ActivateComponent(DealDamageEnum.Ally);

        _dealDamageComponent = _enemyView.dealDamageComponent;
        _dealDamageComponent.ActivateComponent(10, DealDamageEnum.Enemy);

        _targetFinderComponentFoAttack = _enemyView.targetFinderComponentFoAttack;
        _targetFinderComponentFoAttack.ActivateComponent(typeof(AllyUnitView));

        _targetFinderComponentForMovement = _enemyView.targetFinderComponentForMovement;
        _targetFinderComponentForMovement.ActivateComponent(typeof(AllyUnitView));

        _moveComponent = _enemyView.moveComponent;

        _meleeAtackService ??= _serviceFabric.InitMultiple<MeleeAtackService>();
        _meleeAtackService.ActivateService(
            _targetFinderComponentFoAttack,
            _enemyView.transform);
        _moveService ??= _serviceFabric.InitMultiple<MoveService>();
        _moveService.ActivateService(
            _targetFinderComponentForMovement,
            _moveComponent,
            1f,
            1f);
        

    }
    
    public void DeactivateService()
    {
        Transform pool = _markerService.GetTransformMarker<ViewPoolMarker>();
        _enemyView.DiactivateView(pool);
        _hpComponent.DieAction -= DeactivateService;
        _disposables.Dispose();
        _moveService.DeactivateService();
        _meleeAtackService.DeactivateService();
        DeactivateServiceToPool();
    }
}



