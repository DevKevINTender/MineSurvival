using UniRx;

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
        _targetFinderComponent.ActivateComponent(typeof(EnemyUnitView));

        _hpComponent = _unitView.GetComponentInChildren<HpComponent>();
        _hpComponent.ActivateComponent(50);
        _hpComponent.DieAction += OnDieAction;

        _takeDamageComponent = _unitView.GetComponentInChildren<TakeDamageComponent>();
        _takeDamageComponent.ActivateComponent(DealDamageEnum.Enemy);

        _rangeAttackService = _serviceFabric.InitMultiple<RangeAttackService>();
        _rangeAttackService.ActivateService(_targetFinderComponent, _unitView.gunBarrel);
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

    public void OnDieAction()
    {
        DeactivateService();
    }

    public override void DeactivateService()
    {
        _unitView.Disable();
        _hpComponent.DieAction -= OnDieAction;
        _rangeAttackService.OnStartShootAction -= _unitView.StartShoot;
        _rangeAttackService.OnStopShootAction -= _unitView.StopShoot;
        _rangeAttackService.DeactivateService();
        _disposables.Dispose();
    }
}
