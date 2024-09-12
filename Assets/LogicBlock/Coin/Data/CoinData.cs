using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

[JsonObject(MemberSerialization.OptIn)]
[CreateAssetMenu(fileName = "CoinData", menuName = "ScriptableObjects/CoinData", order = 1)]
public class CoinData : ScriptableObject
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public ReactiveProperty<BigNumber> Count = new();
}