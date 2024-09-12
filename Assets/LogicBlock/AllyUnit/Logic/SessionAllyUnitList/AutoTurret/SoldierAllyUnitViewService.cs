using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

public class SoldierAllyUnitViewService : AllyUnitViewService
{
    [Inject] private IPoolsViewService _poolsViewService;
    private List<EnemyView> targetPool = new();
    private EnemyTargetFinderComponent targetFinderView;
    private HpComponent _hpComponent;
    private AllyTakeDamageComponent _takeDamage;
    private readonly ReactiveProperty<float> _intervalProperty = new ReactiveProperty<float>();
    private float _shootInterval = 0.1f;
    private SoldierAllyUnitView _soldierAllyUnitView;
    private IPoolViewService _bulletPoolViewService;
    private List<AllyBuletViewService> _bulletList = new();

    public override void ActivateService(AllyUnitData unitData)
    {
        base.ActivateService(unitData);
        _soldierAllyUnitView = base._skillView as SoldierAllyUnitView;
        _soldierAllyUnitView.OnLostTargetAction = OnLostTargetAction;

        Transform parent = _soldierAllyUnitView.transform;
        targetFinderView = _viewFabric.Init<EnemyTargetFinderComponent>(parent);
        targetFinderView.OnFindNewTargetAction = OnFindNewTargetaction;

        _hpComponent = _soldierAllyUnitView.GetComponentInChildren<HpComponent>();
        _hpComponent.ActivateComponent(100);
        _hpComponent.DieAction += OnDieAction;
        _takeDamage = _soldierAllyUnitView.GetComponentInChildren<AllyTakeDamageComponent>();
        _takeDamage.ActivateComponent<EnemyDealDamageComponent>();

        _bulletPoolViewService = _poolsViewService.GetPool<AllyBuletViewService, AllyBulletView>();

        _intervalProperty.Value = _shootInterval;

        _intervalProperty
            .Select(interval => Observable.Interval(TimeSpan.FromSeconds(interval)))
            .Switch() // Переключаемся на новый поток интервалов при изменении интервала
            .Subscribe(_ =>
            {
                targetPool.RemoveAll(item => item == null);
                if (targetPool.Any()) Shoot();
            })
            .AddTo(_disposables);

        _currentStatus
            .Subscribe(x =>
            {
                switch (x)
                {
                    case SkillStatus.Active:
                        //_autoTurretSkillView.Enable();
                        break;
                    case SkillStatus.Recovery:
                        //_autoTurretSkillView.Disable();
                        break;
                    case SkillStatus.ReadyToActive:
                        break;
                }
            })
            .AddTo(_disposables);
    }

    public void OnDieAction()
    {
        _soldierAllyUnitView.Disable();
    }

    public void Shoot()
    {
        AllyBuletViewService bullet = (AllyBuletViewService)_bulletPoolViewService.GetItem<AllyBulletView>();
        bullet.ActivateService(_soldierAllyUnitView.transform.position, targetPool.FirstOrDefault().transform.position, 10);
    }

    public void OnFindNewTargetaction(EnemyView enemy)
    {
        targetPool.Add(enemy);
    }

    public void OnLostTargetAction()
    {
        targetPool.RemoveAll(item => item == null);
        _soldierAllyUnitView.SetTarget(targetPool.FirstOrDefault()?.transform);
    }

    public override void DeactivateService()
    {
        _disposables.Dispose();
        _hpComponent.DieAction -= OnDieAction;
    }
}

public enum SkillStatus
{
    Active,
    Recovery,
    ReadyToActive
}

