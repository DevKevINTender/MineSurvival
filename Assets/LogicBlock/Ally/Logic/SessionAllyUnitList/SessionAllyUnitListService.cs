using Zenject;
using System.Collections.Generic;
using System;

public class SessionAllyUnitListService
{
    [Inject] private IServiceFabric _serviceFabric;
    [Inject] private AllyUnitDataManager _allyUnitDataManager;
    private List<AllyUnitViewService> _unitViewServiceList = new List<AllyUnitViewService>();
    private AllyUnitData[] _sessionAllyUnitDataArray;
    private Dictionary<AllyUnitEnum, System.Type> _keyValuePairs = new Dictionary<AllyUnitEnum, System.Type>()
    {
        { AllyUnitEnum.Fisher, typeof(DefenseAllyUnitViewService) },
        { AllyUnitEnum.Banatic, typeof(AllyUnitViewService) },
        { AllyUnitEnum.Ice, typeof(AttackAllyUnitViewService) }
    };

    public void ActivateService()
    {
        _sessionAllyUnitDataArray = _allyUnitDataManager.GetAllyUnitDataArray();
        foreach(AllyUnitData item in _sessionAllyUnitDataArray)
        {
            AllyUnitViewService newUnitViewService = (AllyUnitViewService)_serviceFabric.InitMultiple(_keyValuePairs[item.Name]);
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

