using UnityEngine;
using System.Collections.Generic;
using System;
using Zenject;

[CreateAssetMenu(fileName = "EnemyListData", menuName = "ScriptableObjects/EnemyListData", order = 1)]
public class EnemyListData : ScriptableObject
{
    public List<EnemyData> enemyDatas = new List<EnemyData>();
}

[Serializable]
public class EnemyData
{
    public int id;
    public GameObject prefab;
}

public class EnemyListDataManager
{
    private EnemyListData EnemyListData;
    private ISOStorageService _storageService;

    [Inject]
    public EnemyListDataManager(ISOStorageService storageService)
    {
        _storageService = storageService;
        EnemyListData = _storageService.GetSOByType<EnemyListData>();

    }

    public EnemyListData GetEnemyListData() => EnemyListData;
}


