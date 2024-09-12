using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class UpgradeDataManager
{
    private CoinDataManager _coinDataManager;
    private ItemPoolDataManager _itemPoolDataManager;
    private List<IDisposable> _disposable = new();
    private CoinData _coinData;

    [Inject]
    public UpgradeDataManager(CoinDataManager coinDataManager, ItemPoolDataManager itemPoolDataManager)
    {
        _coinDataManager = coinDataManager;
        _coinData = _coinDataManager.GetCoinData();
        _itemPoolDataManager = itemPoolDataManager;
        _coinData.Count.Subscribe((newValue) => UpdateItemCanUpgrade()).AddTo(_disposable);

    }

    public void Dispose()
    {
        foreach (var item in _disposable)
        {
            item.Dispose();
        }
    }

    public void UpgradeItem(ItemData itemData)
    {
        if (itemData.CostType == CostTypeEnum.Ads)
        {
            //запуск рекламы
            itemData.Upgrade();
        }
        else
        {
            if (itemData.CurrentCost <= _coinData.Count.Value)
            {
                _coinDataManager.WithdrawCoin(itemData.CurrentCost);
                itemData.Upgrade();
            }
        }
      
    }

    public void UpdateItemCanUpgrade()
    {
        foreach (var item in _itemPoolDataManager.ItemPoolData.ItemDatas)
        {
            item.CanUpgrade.Value = (item.CurrentCost <= _coinData.Count.Value);
        }
    }
}
