using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DefaultLevelData", menuName = "ScriptableObjects/DefaultLevelData", order = 1)]
public class DefaultLevelData: ScriptableObject
{
    public int CurrentLevelId = 0;
    [SerializeField]
    public List<LevelData> LevelList = new List<LevelData>();
}



