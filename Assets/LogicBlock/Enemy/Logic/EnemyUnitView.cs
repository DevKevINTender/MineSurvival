using Zenject;
using UnityEngine;
using UniRx;
using UnityEditor;

public class EnemyUnitView: MonoBehaviour
{
    public HpComponent hpComponent;
    public MoveComponent moveComponent;
    public TakeDamageComponent takeDamageComponent;
    public TargetFinderComponent targetFinderComponentForMovement;
    public TargetFinderComponent targetFinderComponentFoAttack;
    public MeleeProjectileView MeleeProjectileViewPrefab;
    [SerializeField] private Animator _animator;

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

    public void TakeDamage()
    {
        _animator.Play("TakeDamage");
    }
}

public class EnemyUnitViewService : PoolingViewService
{
	[Inject] private IViewFabric _viewFabric;
	[Inject] private IServiceFabric _serviceFabric;
    [Inject] private IMarkerService _markerService;
    [Inject] private EnemyListDataManager _enemyListDataManager;

    private EnemyData _enemyData;
    private EnemyUnitView _enemyView;
    private Transform _spawnPos;

    private MeleeAtackService _meleeAtackService;
    private MoveService _moveService;

    private CompositeDisposable _disposables = new();

    public void ActivateService(EnemyEnum enemyName)
    {
        _enemyData = _enemyListDataManager.GetEnemyDataByName(enemyName);

        _spawnPos = _markerService.GetTransformMarker<EnemySpawnPosMarker>();
        _enemyView ??= _viewFabric.Init<EnemyUnitView>(_enemyData.prefab);
        _enemyView.ActivateView(_spawnPos);

        _enemyView.hpComponent.ActivateComponent(250);
        _enemyView.hpComponent.DieAction += DeactivateService;
        _enemyView.hpComponent.TakeDamageAction += _enemyView.TakeDamage;

        _enemyView.takeDamageComponent.ActivateComponent(DealDamageType.Ally);
        _enemyView.targetFinderComponentFoAttack.ActivateComponent(typeof(AllyUnitView));
        _enemyView.targetFinderComponentForMovement.ActivateComponent(typeof(AllyUnitView));

        _meleeAtackService ??= _serviceFabric.InitMultiple<MeleeAtackService>();
        _meleeAtackService.ActivateService(
            _enemyView.targetFinderComponentFoAttack,
            _enemyView.transform,
            DealDamageType.Enemy,
            _enemyView.MeleeProjectileViewPrefab);
        _moveService ??= _serviceFabric.InitMultiple<MoveService>();
        _moveService.ActivateService(
            _enemyView.targetFinderComponentForMovement,
            _enemyView.moveComponent,
            1f,
            1f);
    }
    
    public void DeactivateService()
    {
        _disposables.Dispose();
        _moveService.DeactivateService();
        _meleeAtackService.DeactivateService();
        _enemyView.hpComponent.DieAction -= DeactivateService;
        _enemyView.hpComponent.TakeDamageAction -= _enemyView.TakeDamage;
        _enemyView.targetFinderComponentFoAttack.DeactivateComponent();
        _enemyView.targetFinderComponentForMovement.DeactivateComponent();
        _enemyView.DiactivateView(viewPool);
        DeactivateServiceToPool();
    }
}



