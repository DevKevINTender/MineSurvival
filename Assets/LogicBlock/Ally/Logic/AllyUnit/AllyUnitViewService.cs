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
    [Inject] protected AllyUnitSlotArrayViewService _slotArrayViewService;
    protected AllyUnitData _skillData;
    protected AllyUnitView _allyUnitView;
    protected AllyUnitSkillButtonService _allySkillButtonService;
    protected ReactiveProperty<SkillStatus> _currentStatus = new();
    protected CompositeDisposable _disposables = new();

    public virtual void ActivateService(AllyUnitData unitData)
    {
        _skillData = unitData;
        Transform viewPos = _slotArrayViewService.GetAllyUnitSlotTransformByName(_skillData.Name);
        _allyUnitView = _viewFabric.Init(_skillData.Prefab, viewPos);

        _allySkillButtonService = _serviceFabric.InitMultiple<AllyUnitSkillButtonService>();
        _allySkillButtonService.ActivateService(unitData);
        _currentStatus = _allySkillButtonService._currentStatus;
    }


    public virtual void DeactivateService()
    {
        _disposables.Dispose();
    }
}

