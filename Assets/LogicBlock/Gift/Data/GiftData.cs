using UnityEngine;
using System;
using Newtonsoft.Json;

[CreateAssetMenu(fileName = "CoinGiftData", menuName = "ScriptableObjects/CoinGiftData", order = 1)]
public class GiftData : ScriptableObject
{
    public int count = 0;
    public GiftEnum type;
    public Sprite icon;
}

public enum GiftEnum
{
    Coin,
    Gem
}


public interface IGiftService
{
    public void TakeGift();
}






