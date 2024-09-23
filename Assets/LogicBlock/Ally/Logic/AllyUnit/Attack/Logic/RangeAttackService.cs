using System;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

public class RangeAttackService
{
    public Action OnStartShootAction;
    public Action OnStopShootAction;
    [Inject] private IViewServicePoolService _poolsViewService;
    private IViewServicePool _bulletPoolViewService;
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

        _bulletPoolViewService = _poolsViewService.GetPool<RangeProjectileViewService>();

        _intervalProperty
            .Select(interval => Observable.Interval(TimeSpan.FromSeconds(interval)))
            .Switch() // Переключаемся на новый поток интервалов при изменении интервала
            .Subscribe(_ =>
            {
                if (_targetFinderComponent.CurrentTarget.Value != null)
                {
                    Shoot();
                    OnStartShootAction?.Invoke();
                }
                else
                {
                    OnStopShootAction?.Invoke();
                }
            })
            .AddTo(_disposables);
    }
    private void Shoot()
    {
        
        RangeProjectileViewService bullet = (RangeProjectileViewService)_bulletPoolViewService.GetItem();
        bullet.ActivateService(_spawPos.position, _targetFinderComponent.CurrentTarget.Value.transform.position, 10);
    }

    public void DeactivateService()
    {
        _disposables.Dispose();
    }
}
