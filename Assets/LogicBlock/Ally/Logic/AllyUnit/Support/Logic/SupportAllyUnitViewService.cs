using UniRx;

public class SupportAllyUnitViewService : AllyUnitViewService
{
    private SupportAllyUnitView _unitView;
    private HealService _healService;

    public override void ActivateService(AllyUnitData unitData)
    {

        base.ActivateService(unitData);
        _unitView = base._allyUnitView as SupportAllyUnitView;
        _unitView.ActivateView(_currentStatus, unitData);

        _unitView.TargetFinderComponent.ActivateComponent(DealHealType.Ally);

        _unitView.HpComponent.ActivateComponent(100);
        _unitView.HpComponent.DieAction += OnDieAction;
        _unitView.HpComponent.TakeDamageAction += _unitView.TakeDamage;

        _unitView.TakeDamageComponent.ActivateComponent(DealDamageType.Enemy);
        _unitView.TakeHealComponent.ActivateComponent(DealHealType.Ally);

        _healService = _serviceFabric.InitMultiple<HealService>();
        _healService.ActivateService(
            _unitView.TargetFinderComponent,
            _unitView.HealProjectileViewPreafab);
        _healService.OnStartHealAction += _unitView.StartHeal;
        _healService.OnStopHealktAction += _unitView.StopHeal;


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
        if (_unitView != null) _unitView.Disable();
        _disposables.Dispose();
        _unitView.HpComponent.DieAction -= OnDieAction;
        _unitView.HpComponent.TakeDamageAction -= _unitView.TakeDamage;
        _unitView.TargetFinderComponent.DeactivateComponent();
        _healService.OnStartHealAction -= _unitView.StartHeal;
        _healService.OnStopHealktAction -= _unitView.StopHeal;
    }
}