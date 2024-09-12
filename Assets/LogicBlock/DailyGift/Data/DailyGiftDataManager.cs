using Zenject;
using System;
using UniRx;
using UnityEngine;

public class DailyGiftDataManager
{
    private const string dailyGiftKey = "DailyGift";
    private const string giftKey = "Gift";
    public ReactiveProperty<bool> isTakeToday = new();
    public ReactiveProperty<GiftData> nextGift = new();
    private DailyGiftArrayData _dailyGiftData;
    private ISOStorageService _storageService;
    private IDisposable _subscription;


    [Inject]
    public DailyGiftDataManager(ISOStorageService storageService) 
    {
        _storageService = storageService;
        _dailyGiftData = _storageService.GetSOByType<DailyGiftArrayData>();
        LoadData();
        CheckForNewDay();
        UpdateNextGift();
        StartPeriodicCheck();
    }

    public void LoadData()
    {
        foreach (var item in _dailyGiftData.giftDatas)
        {
            SaveLoader.LoadItem(dailyGiftKey + item.id, item);
        }
        SaveLoader.LoadItem(dailyGiftKey, _dailyGiftData);
    }

    public GiftData TakeCurrentGift()
    {
        isTakeToday.Value = true;
        _dailyGiftData.lastTakeGiftTime = DateTime.Now;
        DailyGiftData currentGift = null;
        foreach (var item in _dailyGiftData.giftDatas)
        {
            if (item.isTaked.Value == false)
            {
                currentGift = item;
                currentGift.isTaked.Value = true;
                SaveData();
                UpdateNextGift();
                return currentGift.giftData;
            }
        }

        return null; 
    }

    public DailyGiftData GetDailyGiftDataById(int id)
    {
        return _dailyGiftData.giftDatas[id];
    }

    private void SaveData()
    {
        foreach (var item in _dailyGiftData.giftDatas)
        {
            SaveLoader.SaveItem(dailyGiftKey + item.id, item);
        }
        SaveLoader.SaveItem(dailyGiftKey, _dailyGiftData);
    }

    private void StartPeriodicCheck()
    {
        _subscription = Observable.Interval(TimeSpan.FromMinutes(1))
            .Subscribe(_ => CheckForNewDay());
    }

    private void CheckForNewDay()
    {
        MonoBehaviour.print($"{_dailyGiftData.lastTakeGiftTime.Date} и {DateTime.Now.Date}");
        if (_dailyGiftData.lastTakeGiftTime.Date != DateTime.Now.Date)
        {
            // Новый день
            isTakeToday.Value = false;
        }
        else
        {
            isTakeToday.Value = true;
        }
    }

    public void Dispose()
    {
        _subscription?.Dispose();
    }

    public void UpdateNextGift()
    {
        foreach (var item in _dailyGiftData.giftDatas)
        {
            if (item.isTaked.Value == false)
            {
                nextGift.Value = item.giftData;
                return;
            }
        }
        nextGift.Value = null;
    }
}
