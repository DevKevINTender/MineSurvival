using System;
using System.Collections.Generic;
using Zenject;

public class DefaultLevelDataManager
{
    public DefaultLevelData DefaultLevelData;
    private ISOStorageService _storageService;

    [Inject]
    public DefaultLevelDataManager(ISOStorageService storageService)
    {
        _storageService = storageService;
        DefaultLevelData = _storageService.GetSOByType<DefaultLevelData>();
    }

    public DefaultLevelData GetDefaultLevelData() => DefaultLevelData;

    public LevelData GetCurrentLevelData()
    {
        return DefaultLevelData.LevelList[DefaultLevelData.CurrentLevelId];
    }

    public List<EnemyStageData> GetEnemyStageDataList()
    {
        return DefaultLevelData.LevelList[DefaultLevelData.CurrentLevelId].EnemyStageList;
    }
}



