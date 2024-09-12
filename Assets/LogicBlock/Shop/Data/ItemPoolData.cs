using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System;
using UniRx;
using UnityEngine;

[JsonObject(MemberSerialization.OptIn)]
[CreateAssetMenu(fileName = "ItemPoolData", menuName = "ScriptableObjects/ItemPoolData", order = 1)]
public class ItemPoolData: ScriptableObject
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [ListDrawerSettings(Expanded = true, NumberOfItemsPerPage = 3)]
    [SerializeField] public ItemData[] ItemDatas;

    [ContextMenu("AutoID")]
    private void AutoId()
    {
        for (int i = 0; i < ItemDatas.Length; i++)
        {
            ItemDatas[i].Id = i;
        }
    }
}

[Serializable]
public class ItemData
{
    [SerializeField] public int Id;
    [SerializeField] public string Name;
    [PreviewField(50)]
    [SerializeField] public Sprite Icon;
    [PreviewField(50)]
    [SerializeField] public Sprite Grade;
    [SerializeField] public int RequiredLevel;
    [SerializeField] public CostTypeEnum CostType;
    [SerializeField] public IncomeTypeEnum IncomeType;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [SerializeField] public IntReactiveProperty Count = new IntReactiveProperty(0);
    [SerializeField] private int _maxCount;

    [SerializeField] private BigNumber _baseIncome = new BigNumber(10,0);
    [SerializeField] private BigNumber _baseCost = new BigNumber(1,0);
    public BigNumber CurrentIncome { get => _baseIncome * new BigNumber(Count.Value + 1, 0);}
    public BigNumber CurrentCost { get => _baseCost * new BigNumber(Count.Value + 1, 0);}
    public BoolReactiveProperty CanUpgrade = new();

    public bool Upgrade()
    {
        Count.Value++;
        return true;
    }
}

public enum CostTypeEnum
{
    Coins,
    Ads
    
}

public enum IncomeTypeEnum
{
    Click,
    Auto
}
