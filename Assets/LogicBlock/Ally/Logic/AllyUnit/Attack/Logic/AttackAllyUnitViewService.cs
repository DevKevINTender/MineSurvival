﻿using UniRx;

public class AttackAllyUnitViewService : AllyUnitViewService
{
    private TargetFinderComponent _targetFinderComponent;
    private HpComponent _hpComponent;
    private TakeDamageComponent _takeDamageComponent;
    private AttackAllyUnitView _unitView;
    private RangeAttackService _rangeAttackService;

    public override void ActivateService(AllyUnitData unitData)
    {
        base.ActivateService(unitData);
        _unitView = base._allyUnitView as AttackAllyUnitView;
        _unitView.ActivateView(_currentStatus, unitData);
        
        _targetFinderComponent = _unitView.GetComponentInChildren<TargetFinderComponent>();
        _targetFinderComponent.ActivateComponent(typeof(EnemyTargetComponent));

        _hpComponent = _unitView.GetComponentInChildren<HpComponent>();
        _hpComponent.ActivateComponent(50);
        _hpComponent.DieAction += DeactivateService;
        _hpComponent.TakeDamageAction += _unitView.TakeDamage;

        _unitView.TakeHealComponent.ActivateComponent(DealHealType.Ally);

        _takeDamageComponent = _unitView.GetComponentInChildren<TakeDamageComponent>();
        _takeDamageComponent.ActivateComponent(DealDamageType.Enemy);

        _rangeAttackService = _serviceFabric.InitMultiple<RangeAttackService>();
        _rangeAttackService.ActivateService(_targetFinderComponent, _unitView.gunBarrel, _unitView.RangeProjectileViewPrefab, DealDamageType.Ally);
        _rangeAttackService.OnStartShootAction += _unitView.StartShoot;
        _rangeAttackService.OnStopShootAction += _unitView.StopShoot;

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

    public override void DeactivateService()
    {
        base.DeactivateService();
        if (_unitView != null) _unitView.DeactivateView();
        _hpComponent.DieAction -= DeactivateService;
        _hpComponent.TakeDamageAction -= _unitView.TakeDamage;
        _targetFinderComponent.DeactivateComponent();
        _rangeAttackService.OnStartShootAction -= _unitView.StartShoot;
        _rangeAttackService.OnStopShootAction -= _unitView.StopShoot;
        _rangeAttackService.DeactivateService();
    }
}
