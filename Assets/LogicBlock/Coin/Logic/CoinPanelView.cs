using Zenject;
using UnityEngine;
using TMPro;
using System;
using UniRx;
using System.Collections.Generic;

public class CoinPanelView : MonoBehaviour
{
    [SerializeField] private TMP_Text countText;

    public void UpdateView(BigNumber newValue)
    {
        countText.text = newValue.ToString();
    }
}

public class CoinPanelService : IService
{
    [Inject] private IViewFabric _fabric;
    [Inject] private IMarkerService _markerService;
    [Inject] private CoinDataManager _coinDataManager;
    private CoinPanelView _coinPanelView;
    private List<IDisposable> _disposables = new();
    private CoinData _coinData;
    public void ActivateService()
    {
        Transform parent = _markerService.GetTransformMarker<LeftTopPanelMarker>();
        _coinPanelView = _fabric.Init<CoinPanelView>(parent);
        _coinData = _coinDataManager.GetCoinData();
        _coinData.Count.Subscribe((newValue) => _coinPanelView.UpdateView(newValue)).AddTo(_disposables);
    }

    public void DeactivateService()
    {
        
    }
}