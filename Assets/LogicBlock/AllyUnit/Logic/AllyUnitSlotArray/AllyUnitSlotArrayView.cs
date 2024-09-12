using UnityEngine;
using Zenject;

public class AllyUnitSlotArrayView : MonoBehaviour
{
    public Transform[] AllyUnitSlotArray;
}

public class AllyUnitSlotArrayViewService
{
    [Inject] private IViewFabric _viewFabric;
    [Inject] private IMarkerService _markerService;
    private AllyUnitSlotArrayView _allyUnitSlotArrayView;
    private Transform[] _allyUnitSlotArray;

    public void ActivateService()
    {
        Transform parent = _markerService.GetTransformMarker<SkillTowerPointPosMarker>();
        _allyUnitSlotArrayView = _viewFabric.Init<AllyUnitSlotArrayView>(parent);
        _allyUnitSlotArray = _allyUnitSlotArrayView.AllyUnitSlotArray;
    }

    public Transform GetAllyUnitSlotTransformById(int id)
    {
        return _allyUnitSlotArray[id];
    }
}
