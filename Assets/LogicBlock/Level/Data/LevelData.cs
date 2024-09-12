using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level", order = 1)]
public class LevelData: ScriptableObject
{
    public int Id;
    public List<EnemySetData> EnemySetList = new();
}

[Serializable]
public class EnemySetData
{
    public int enemyId;
    public int count = 1;
}
