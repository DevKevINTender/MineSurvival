using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level", order = 1)]
public class LevelData: ScriptableObject
{
    public int Id;
    public List<EnemyStageData> EnemyStageList = new();
}