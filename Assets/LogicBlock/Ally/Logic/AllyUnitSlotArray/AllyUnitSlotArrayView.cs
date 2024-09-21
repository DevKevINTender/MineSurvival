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
    [Inject] private AllyUnitDataManager _allyUnitDataManager;
    private AllyUnitSlotArrayView _allyUnitSlotArrayView;
    private Transform[] _allyUnitSlotArray;

    public void ActivateService()
    {
        Transform parent = _markerService.GetTransformMarker<AllyUnitSlotPosMarker>();
        _allyUnitSlotArrayView = _viewFabric.Init<AllyUnitSlotArrayView>(parent);
        _allyUnitSlotArray = _allyUnitSlotArrayView.AllyUnitSlotArray;
    }

    public Transform GetAllyUnitSlotTransformByName(AllyUnitEnum name)
    {
        int sessionId = _allyUnitDataManager.GetSessionIdByEnum(name);
        return _allyUnitSlotArray[sessionId];
    }
}
