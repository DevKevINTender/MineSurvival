using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllyUnitArrayData", menuName = "ScriptableObjects/AllyUnitArrayData", order = 1)]
[JsonObject(MemberSerialization.OptIn)]
public class AllyUnitArrayData : ScriptableObject
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public AllyUnitEnum[] sessionAllyUnitNameArray = new AllyUnitEnum[3];
    public AllyUnitData[] allyUnitDataArray;
}

[Serializable]
[JsonObject(MemberSerialization.OptIn)]
public class AllyUnitData
{
    [JsonIgnore]
    public AllyUnitEnum name;
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public int level;
    [JsonIgnore]
    public AllyUnitView[] prefabs = new AllyUnitView[4];
    [JsonIgnore]
    public Sprite unitIcon;
    [JsonIgnore]
    public float skillDuration = 5f;
    [JsonIgnore]
    public float skillRecovery = 10f;

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


public enum AllyUnitEnum
{
    Banatic,
    Fisher,
    Idea,
    Ice
}




