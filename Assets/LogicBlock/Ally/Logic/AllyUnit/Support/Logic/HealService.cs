using System;
using System.Linq;
using UniRx;
using Zenject;

public class HealService
{
    [Inject] private IViewServicePoolService _poolsViewService;
    private IViewServicePool _projectilePoolViewService;
    private CompositeDisposable _disposables = new();

    public Action OnStartHealAction;
    public Action OnStopHealktAction;

    private HealTargetFinderComponent _targetFinderComponent;
    private HealProjectileView _healProjectilePrefab;

    private readonly ReactiveProperty<float> _intervalProperty = new ReactiveProperty<float>(0.1f);
    public void ActivateService(
       HealTargetFinderComponent targetFinderComponent,
       HealProjectileView healProjectilePrefab)
    {
        _targetFinderComponent = targetFinderComponent;
        _healProjectilePrefab = healProjectilePrefab;

        _projectilePoolViewService = _poolsViewService.GetPool<HealProjectileViewService>();

        _intervalProperty
            .Select(interval => Observable.Interval(TimeSpan.FromSeconds(interval)))
            .Switch() // Переключаемся на новый поток интервалов при изменении интервала
            .Subscribe(_ =>
            {
                if (_targetFinderComponent.CurrentTarget.Value != null)
                {
                    Heal();
                    OnStartHealAction?.Invoke();
                }
                else
                {
                    OnStopHealktAction?.Invoke();
                }
            })
            .AddTo(_disposables);
    }

    private void Heal()
    {

        HealProjectileViewService projecttile = (HealProjectileViewService)_projectilePoolViewService.GetItem();
        projecttile.ActivateService(
            _targetFinderComponent.CurrentTarget.Value.transform.position,
            10,
            _healProjectilePrefab);
    }

    public void DeactivateService()
    {
        _disposables.Dispose();
    }
}


