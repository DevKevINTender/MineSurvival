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
    private readonly ReactiveProperty<float> _intervalProperty = new ReactiveProperty<float>(0.1f);
    private CompositeDisposable _disposables = new();
    private Transform _spawPos;

    public void ActivateService(
        TargetFinderComponent targetFinderComponent,
        Transform spawPos)
    {
        _targetFinderComponent = targetFinderComponent;
        _spawPos = spawPos;

        _projectilePoolViewService = _poolsViewService.GetPool<MeleeProjectileViewService>();

        _intervalProperty
            .Select(interval => Observable.Interval(TimeSpan.FromSeconds(interval)))
            .Switch() // Переключаемся на новый поток интервалов при изменении интервала
            .Subscribe(_ =>
            {
                if (_targetFinderComponent.CurrentTarget != null)
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

        MeleeProjectileViewService bullet = (MeleeProjectileViewService)_projectilePoolViewService.GetItem();
        bullet.ActivateService(_spawPos.position, 10);
    }
}