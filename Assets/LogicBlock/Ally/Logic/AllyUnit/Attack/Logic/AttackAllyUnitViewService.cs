using UniRx;

public class AttackAllyUnitViewService : AllyUnitViewService
{
    private TargetFinderComponent _targetFinderComponent;
    private HpComponent _hpComponent;
    private AllyTakeDamageComponent _takeDamage;
    private AttackAllyUnitView _soldierAllyUnitView;
    private RangeAttackService _shootService;

    public override void ActivateService(AllyUnitData unitData)
    {

        base.ActivateService(unitData);
        _soldierAllyUnitView = base._allyUnitView as AttackAllyUnitView;
        _soldierAllyUnitView.ActivateView(_currentStatus, unitData);
        
        _targetFinderComponent = _soldierAllyUnitView.GetComponentInChildren<TargetFinderComponent>();
        _targetFinderComponent.ActivateComponent(typeof(EnemyUnitView));

        _hpComponent = _soldierAllyUnitView.GetComponentInChildren<HpComponent>();
        _hpComponent.ActivateComponent(100);
        _hpComponent.DieAction += OnDieAction;

        _takeDamage = _soldierAllyUnitView.GetComponentInChildren<AllyTakeDamageComponent>();
        _takeDamage.ActivateComponent<EnemyDealDamageComponent>();

        _shootService = _serviceFabric.InitMultiple<RangeAttackService>();
        _shootService.ActivateService(_targetFinderComponent, _soldierAllyUnitView.gunBarrel);
        _shootService.OnStartShootAction += _soldierAllyUnitView.StartShoot;
        _shootService.OnStopShootAction += _soldierAllyUnitView.StopShoot;


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

    public override void DeactivateService()
    {
        _disposables.Dispose();
        _hpComponent.DieAction -= OnDieAction;
        _shootService.OnStartShootAction -= _soldierAllyUnitView.StartShoot;
        _shootService.OnStopShootAction -= _soldierAllyUnitView.StopShoot;
    }
}
