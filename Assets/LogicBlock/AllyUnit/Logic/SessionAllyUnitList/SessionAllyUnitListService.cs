using Zenject;
using System.Collections.Generic;
using System;

public class SessionAllyUnitListService
{
    [Inject] private IServiceFabric _serviceFabric;
    [Inject] private AllyUnitDataManager _allyUnitDataManager;
    private List<AllyUnitViewService> _unitViewServiceList = new List<AllyUnitViewService>();
    private AllyUnitData[] _sessionAllyUnitDataArray;
    private AllyUnitSlotArrayViewService _allyUnitSlotArrayViewService;
    private Dictionary<AllyUnitEnum, Type> _keyValuePairs = new Dictionary<AllyUnitEnum, Type>()
    {
        { AllyUnitEnum.Fisher, typeof(AllyUnitViewService) },
        { AllyUnitEnum.Banatic, typeof(AllyUnitViewService) },
        { AllyUnitEnum.Idea, typeof(SoldierAllyUnitViewService) }
    };

    public void ActivateService()
    {
        _allyUnitSlotArrayViewService = _serviceFabric.InitSingle<AllyUnitSlotArrayViewService>();
        _allyUnitSlotArrayViewService.ActivateService();


        _sessionAllyUnitDataArray = _allyUnitDataManager.GetAllyUnitDataArray();
        foreach(AllyUnitData item in _sessionAllyUnitDataArray)
        {
            AllyUnitViewService newUnitViewService = (AllyUnitViewService)_serviceFabric.InitMultiple(_keyValuePairs[item.name]);
            newUnitViewService.ActivateService(item);
            _unitViewServiceList.Add(newUnitViewService);
        }
    }

    public void DeactivateService()
    {
        foreach (var item in _unitViewServiceList)
        {
            item.DeactivateService();
        }
    }
}

