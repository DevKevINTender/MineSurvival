using System;
using UniRx;
using System.Linq;
using Zenject;
using UnityEditor.Search;

public class AllyUnitDataManager
{
    private ISOStorageService _storageService;

    private const string ArrayKey = "AllyUnitArray";
    private const string Key = "AllyUnit";
    private AllyUnitArrayData _allyUnitArrayData;

    [Inject]
    public AllyUnitDataManager(ISOStorageService storageService)
    {
        _storageService = storageService;
        _allyUnitArrayData = _storageService.GetSOByType<AllyUnitArrayData>();
        LoadData();
    }

    public void LoadData()
    {
        foreach (var item in _allyUnitArrayData.allyUnitDataArray)
        {
            SaveLoader.LoadItem(ArrayKey + item.Name, item);
        }
        SaveLoader.LoadItem(Key, _allyUnitArrayData);
    }

    private void SaveData()
    {
        foreach (var item in _allyUnitArrayData.allyUnitDataArray)
        {
            SaveLoader.SaveItem(ArrayKey + item.Name, item);
        }
        SaveLoader.SaveItem(Key, _allyUnitArrayData);
    }

    public AllyUnitData[] GetAllyUnitDataArray()
    {
        AllyUnitData[] sessionAllyUnitDataArray = _allyUnitArrayData.allyUnitDataArray
           .Where(a => _allyUnitArrayData.sessionAllyUnitNameArray.Contains(a.Name))
           .ToArray();

        return sessionAllyUnitDataArray;
    }

    public int GetSessionIdByEnum(AllyUnitEnum name)
    {
        return Array.FindIndex(_allyUnitArrayData.sessionAllyUnitNameArray, unit => unit == name);
    }
}

