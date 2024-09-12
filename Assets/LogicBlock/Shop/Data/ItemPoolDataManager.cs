using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

public class ItemPoolDataManager
{
    private const string key = "Item";
    [SerializeField] public ItemPoolData ItemPoolData;
    private ISOStorageService _storageService;
    private List<IDisposable> _disposables = new();
    public ReactiveProperty<ItemData> MaxLevelItem = new ();

    [Inject]
    public ItemPoolDataManager(ISOStorageService storageService)
    {
        _storageService = storageService;
        ItemPoolData = _storageService.GetSOByType<ItemPoolData>();
        foreach (ItemData item in ItemPoolData.ItemDatas)
        {
            SaveLoader.LoadItem(key + item.Id, item);
        }

        Observable.Merge(ItemPoolData.ItemDatas.Select(itemData => itemData.Count))
           .Subscribe(_ => RecalculateMaxLevelItem())
           .AddTo(_disposables);
    }

    public void Dispose()
    {
        foreach (var item in _disposables)
        {
            item.Dispose();
        }
    }

    public ItemData[] GetItemList()
    {
        return ItemPoolData.ItemDatas;
    }

    public void SaveData()
    {
        foreach (ItemData item in ItemPoolData.ItemDatas)
        {
            SaveLoader.SaveItem(key + item.Id, item);
        }
    }
   
    private void RecalculateMaxLevelItem()
    {
        MaxLevelItem.Value = ItemPoolData.ItemDatas[0];
        foreach (ItemData item in ItemPoolData.ItemDatas)
        {
            if(item.Count.Value > 0) MaxLevelItem.Value = item;
        }
    }
}
