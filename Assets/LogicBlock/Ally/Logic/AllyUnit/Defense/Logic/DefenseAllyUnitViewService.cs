﻿using UniRx;

public class DefenseAllyUnitViewService : AllyUnitViewService
{
    private TargetFinderComponent _targetFinderComponent;
    private ShieldHpComponent _hpComponent;
    private AllyTakeDamageComponent _takeDamage;
    private DefenseAllyUnitView _unitView;
    private MeleeAtackService _attackService;

    public override void ActivateService(AllyUnitData unitData)
    {

        base.ActivateService(unitData);
        _unitView = base._allyUnitView as DefenseAllyUnitView;
        _unitView.ActivateView(_currentStatus, unitData);

        _targetFinderComponent = _unitView.GetComponentInChildren<TargetFinderComponent>();
        _targetFinderComponent.ActivateComponent(typeof(EnemyUnitView));

        _hpComponent = _unitView.GetComponentInChildren<ShieldHpComponent>();
        _hpComponent.ActivateComponent(100, 250);
        _hpComponent.DieAction += OnDieAction;

        _takeDamage = _unitView.GetComponentInChildren<AllyTakeDamageComponent>();
        _takeDamage.ActivateComponent<EnemyDealDamageComponent>();

        _attackService = _serviceFabric.InitMultiple<MeleeAtackService>();
        _attackService.ActivateService(_targetFinderComponent, _unitView.transform);
        _attackService.OnStartAttackAction += _unitView.StartShoot;
        _attackService.OnStopAttacktAction += _unitView.StopShoot;


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
        _unitView.Disable();
    }

    public override void DeactivateService()
    {
        _disposables.Dispose();
        _hpComponent.DieAction -= OnDieAction;
        _attackService.OnStartAttackAction -= _unitView.StartShoot;
        _attackService.OnStopAttacktAction -= _unitView.StopShoot;
    }
}
