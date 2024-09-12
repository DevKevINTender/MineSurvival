using UniRx;
using UnityEngine;
using Zenject;

public class AllyUnitView: MonoBehaviour
{

}

public class AllyUnitViewService
{
    [Inject] protected IViewFabric _viewFabric;
    [Inject] protected IServiceFabric _serviceFabric;
    [Inject] protected IMarkerService _markerService;
    [Inject] private AllyUnitDataManager _allyUnitDataManager;
    protected AllyUnitData _skillData;
    protected AllyUnitSkillButtonView _skillButtonView;
    protected AllyUnitView _skillView;
    protected ReactiveProperty<SkillStatus> _currentStatus = new();
    protected ReactiveProperty<float> _currentSkillRecovery = new(0);
    protected ReactiveProperty<float> _currentSkillDuration = new(0);
    protected CompositeDisposable _disposables = new();
    protected AllyUnitSlotArrayViewService _slotArrayViewService;

    public virtual void ActivateService(AllyUnitData skillData)
    {
        _slotArrayViewService = _serviceFabric.InitSingle<AllyUnitSlotArrayViewService>();
        _skillData = skillData;
        int sessionId = _allyUnitDataManager.GetIdByEnum(_skillData.name);
        Transform viewPos = _slotArrayViewService.GetAllyUnitSlotTransformById(sessionId);
        _skillView = _viewFabric.Init(_skillData.prefab, viewPos);
        Transform buttonPos = _markerService.GetTransformMarker<SkillButtonViewPanelMarker>();
        _skillButtonView = _viewFabric.Init<AllyUnitSkillButtonView>(buttonPos);
        _skillButtonView.OnActivateSkillAction = OnActivateSkillAction;
        _skillButtonView.ActivateView(_currentStatus, _skillData, _currentSkillRecovery, _currentSkillDuration);

        _currentStatus
           .Subscribe(x =>
           {
               switch (x)
               {
                   case SkillStatus.Active:
                       break;
                   case SkillStatus.Recovery:
                       RecoverySkill();
                       break;
                   case SkillStatus.ReadyToActive:
                       break;
               }
           })
           .AddTo(_disposables);

        _currentStatus.Value = SkillStatus.Recovery;
    }

    public virtual void OnActivateSkillAction()
    {
        if (_currentStatus.Value != SkillStatus.ReadyToActive) return;

        _currentStatus.Value = SkillStatus.Active;

        Observable.Timer(System.TimeSpan.Zero, System.TimeSpan.FromSeconds(1))
            .Select(x => (int)x)
            .TakeWhile(x => x < _skillData.skillDuration)
            .Subscribe(x =>
            {
                _currentSkillDuration.Value = x;
            },
            () =>
            {
                _currentStatus.Value = SkillStatus.Recovery;
            })
            .AddTo(_disposables);
    }

    private void RecoverySkill()
    {

        Observable.Timer(System.TimeSpan.Zero, System.TimeSpan.FromSeconds(1))
            .Select(x => (int)x)
            .TakeWhile(x => x < _skillData.skillRecovery)
            .Subscribe(x =>
            {
                _currentSkillRecovery.Value = x;
            },
            () =>
            {
                _currentStatus.Value = SkillStatus.ReadyToActive;
            })
            .AddTo(_disposables);
    }

    public virtual void DeactivateService()
    {
        _disposables.Dispose();
    }
}

