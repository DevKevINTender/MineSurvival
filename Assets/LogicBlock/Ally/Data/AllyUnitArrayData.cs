using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UniRx;
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
    public AllyUnitEnum Name;
    [JsonIgnore]
    public AllyUnitView Prefab;
    public AllyUnitAtributes UnitAtributes;
    [JsonIgnore]
    public Sprite SkillUnitIcon;
    [JsonIgnore]
    public float SkillDuration = 5f;
    [JsonIgnore]
    public float SkillRecovery = 10f;
}

[Serializable]
[JsonObject(MemberSerialization.OptIn)]
public class AllyUnitAtributes
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public ReactiveProperty<int> level = new(0);

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public ReactiveProperty<int> tier = new(1);

    public AllyUnitCoreAtributeEnum MainAtribute;

    [FoldoutGroup("Strength")]
    public int basicStrength;
    [FoldoutGroup("Strength")]
    public int stepStrengthByLevel;
    public float currentStrength => basicStrength + level.Value * stepStrengthByLevel;

    [FoldoutGroup("Agility")]
    public int basicAgility;
    [FoldoutGroup("Agility")]
    public int stepAgilityByLevel;
    public float currentAgility { get => basicAgility + level.Value * stepAgilityByLevel; }

    [FoldoutGroup("Intelligence")]
    public int basicIntelligence;
    [FoldoutGroup("Intelligence")]
    public int stepIntelligencehByLevel;
    public float currentIntelligence { get => basicIntelligence + level.Value * stepIntelligencehByLevel; }

    public List<AllyUnitAtribute> allyUnitAtributeList;
}

[Serializable]
public class AllyUnitAtribute
{
    public AllyUnitAtributeEnum name;
    public AllyUnitCoreAtributeEnum coreAtribute;
    public float basic;
    public float stepByCoreAtribute;
    public float GetCurrent(float coreAtribute) => basic + coreAtribute * stepByCoreAtribute;
}

public enum AllyUnitCoreAtributeEnum
{
    Strength,
    Agility,
    Intelligence
}
public enum AllyUnitAtributeEnum
{
    AttackRange,
    HealthPoints,
    ShieldPoints
}

public enum AllyUnitEnum
{
    Banatic,
    Fisher,
    Idea,
    Ice
}




