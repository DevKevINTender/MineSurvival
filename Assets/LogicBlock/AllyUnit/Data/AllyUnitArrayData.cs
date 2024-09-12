using Newtonsoft.Json;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AllyUnitArrayData", menuName = "ScriptableObjects/AllyUnitArrayData", order = 1)]
[JsonObject(MemberSerialization.OptIn)]
public class AllyUnitArrayData : ScriptableObject
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public AllyUnitEnum[] sessionAllyUnitNameArray = new AllyUnitEnum[3];
    public AllyUnitData[] allyUnitDataArray;
}

public class SoldierAllyUnitData : AllyUnitData
{
    public float basicRange;
    public float stepRangeByLevel;
    public float currentRange { get => basicRange + level * stepRangeByLevel; }


    public float basicAttack;
    public float stepAttackByLevel;
    public float currentAttack { get => basicAttack + level * stepAttackByLevel; }


    public float basicAttackRate;
    public float stepAttackRateByLevel;
    public float currentAttackRate { get => basicAttackRate + level * stepAttackRateByLevel; }
}

[Serializable]
[JsonObject(MemberSerialization.OptIn)]
public class AllyUnitData : UnitData
{
    [JsonIgnore]
    public AllyUnitEnum name;
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public int level;  
    [JsonIgnore]
    public AllyUnitView prefab;
    [JsonIgnore]
    public Sprite[] unitTierIconArray = new Sprite[4];
    [JsonIgnore]
    public float skillDuration = 5f;
    [JsonIgnore]
    public float skillRecovery = 10f;
}

public class UnitData
{

}

public enum AllyUnitEnum
{
    Banatic,
    Fisher,
    Idea
}




