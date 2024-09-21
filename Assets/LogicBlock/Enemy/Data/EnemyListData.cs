using UnityEngine;
using System.Collections.Generic;
using System;
using Zenject;
using System.Linq;

[CreateAssetMenu(fileName = "EnemyListData", menuName = "ScriptableObjects/EnemyListData", order = 1)]
public class EnemyListData : ScriptableObject
{
    public List<EnemyData> enemyDatas = new List<EnemyData>();
}

[Serializable]
public class EnemyData
{
    public EnemyEnum name;
    public GameObject prefab;
    public EnemyStatsData stats;
}

[Serializable]
public class EnemyStatsData
{
    public int hp;
    public int damage;
    public float moveSpeed;
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

    public EnemyData GetEnemyDataByName(EnemyEnum name) => EnemyListData.enemyDatas.FirstOrDefault(e => e.name == name);
}

public enum EnemyEnum
{
    Default
}


