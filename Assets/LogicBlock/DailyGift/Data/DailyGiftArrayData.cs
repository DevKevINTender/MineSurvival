using UnityEngine;
using System;
using Newtonsoft.Json;
using static SaveLoader;
using UniRx;

[JsonObject(MemberSerialization.OptIn)]
[CreateAssetMenu(fileName = "DailyGiftData", menuName = "ScriptableObjects/DailyGiftData", order = 1)]
public class DailyGiftArrayData: ScriptableObject
{
    public DailyGiftData[] giftDatas = new DailyGiftData[3];
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime lastTakeGiftTime;
}

[Serializable] [JsonObject(MemberSerialization.OptIn)]
public class DailyGiftData
{
    public int id;
    [JsonSerialize] [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public ReactiveProperty<bool> isTaked = new ReactiveProperty<bool>();
    [JsonIgnore]
    public GiftData giftData;
}
