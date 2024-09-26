using System;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

public class MeleeAtackService
{
    public Action OnStartAttackAction;
    public Action OnStopAttacktAction;
    [Inject] private IViewServicePoolService _poolsViewService;
    private IViewServicePool _projectilePoolViewService;
    private TargetFinderComponent _targetFinderComponent;
    private readonly ReactiveProperty<float> _intervalProperty = new ReactiveProperty<float>(1f);
    private CompositeDisposable _disposables = new();
    private Transform _spawPos;
    private DealDamageType _dealDamageType;
    private MeleeProjectileView _meleeProjectilePrefab;

    public void ActivateService(
        TargetFinderComponent targetFinderComponent,
        Transform spawPos,
        DealDamageType dealDamageType,
        MeleeProjectileView meleeProjectilePrefab)
    {
        _targetFinderComponent = targetFinderComponent;
        _spawPos = spawPos;
        _dealDamageType = dealDamageType;
        _meleeProjectilePrefab = meleeProjectilePrefab;

        _projectilePoolViewService = _poolsViewService.GetPool<MeleeProjectileViewService>();

        _intervalProperty
            .Select(interval => Observable.Interval(TimeSpan.FromSeconds(interval)))
            .Switch() // Переключаемся на новый поток интервалов при изменении интервала
            .Subscribe(_ =>
            {
                if (_targetFinderComponent.CurrentTarget.Value != null)
                {
                    Attack();
                    OnStartAttackAction?.Invoke();
                }
                else
                {
                    OnStopAttacktAction?.Invoke();
                }
            })
            .AddTo(_disposables);
    }
    private void Attack()
    {

        MeleeProjectileViewService projecttile = (MeleeProjectileViewService)_projectilePoolViewService.GetItem();
        projecttile.ActivateService(
            _spawPos.position,
            10,
            _dealDamageType,
            _meleeProjectilePrefab);
    }

    public void DeactivateService()
    {
       _disposables.Dispose();
    }
}