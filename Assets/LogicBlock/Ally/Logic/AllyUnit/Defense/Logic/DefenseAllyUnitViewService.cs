using UniRx;

public class DefenseAllyUnitViewService : AllyUnitViewService
{
    private TargetFinderComponent _targetFinderComponent;
    private ShieldHpComponent _hpComponent;
    private TakeDamageComponent _takeDamage;
    private DefenseAllyUnitView _unitView;
    private MeleeAtackService _attackService;

    public override void ActivateService(AllyUnitData unitData)
    {

        base.ActivateService(unitData);
        _unitView = base._allyUnitView as DefenseAllyUnitView;
        _unitView.ActivateView(_currentStatus, unitData);

        _targetFinderComponent = _unitView.GetComponentInChildren<TargetFinderComponent>();
        _targetFinderComponent.ActivateComponent(typeof(EnemyTargetComponent));

        _hpComponent = _unitView.GetComponentInChildren<ShieldHpComponent>();
        _hpComponent.ActivateComponent(100, 250);
        _hpComponent.DieAction += OnDieAction;

        _takeDamage = _unitView.GetComponentInChildren<TakeDamageComponent>();
        _takeDamage.ActivateComponent(DealDamageEnum.Enemy);

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
        DeactivateService();
    }

    public override void DeactivateService()
    {
        if(_unitView != null) _unitView.Disable();
        _disposables.Dispose();
        _hpComponent.DieAction -= OnDieAction;
        _attackService.OnStartAttackAction -= _unitView.StartShoot;
        _attackService.OnStopAttacktAction -= _unitView.StopShoot;
    }
}
