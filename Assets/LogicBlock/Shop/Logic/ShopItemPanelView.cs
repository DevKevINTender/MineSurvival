using Zenject;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UniRx;
using System.Collections.Generic;

public class ShopItemPanelView : MonoBehaviour
{
    public Action OnUpgradeAction;

    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text incomeText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text countText;
    [SerializeField] private Image IconImage;
    [SerializeField] private Image GradeImage;

    [SerializeField] private GameObject[] costTypeImageTypes;
    [SerializeField] private GameObject[] incomeTypeImageTypes;

    [SerializeField] private GameObject _notEnoughLevel;
    [SerializeField] private TMP_Text _notEnoughLevelText;
    [SerializeField] private GameObject _itemCostStatusText;
    public Button upgradeButton;

    private ItemData _itemData;


    public void ActivateView(ItemData itemData)
    {
        _itemData = itemData;
        nameText.text = _itemData.Name;
        IconImage.sprite = _itemData.Icon;
        GradeImage.sprite = _itemData.Grade;

        upgradeButton.onClick.AddListener(OnUpgradeAction.Invoke);

        _itemData.Count.Subscribe(newValue =>
        {
            countText.text = $"x{newValue}";
            incomeText.text = $"+{_itemData.CurrentIncome}";
            costText.text = $"{_itemData.CurrentCost}";
        }).AddTo(this);

        _itemData.CanUpgrade.Subscribe(status =>
        {
            if(itemData.CostType == CostTypeEnum.Ads)
            {
                _itemCostStatusText.SetActive(false);
            }
            else
            {
                _itemCostStatusText.SetActive(status == false);
            }
        });


        ChooseCostTypeObject();
        ChooseIncomeTypeObject();
    }

    private void ChooseCostTypeObject()
    {
        foreach (GameObject item in costTypeImageTypes)
        {
            item.SetActive(false);
        }
        costTypeImageTypes[(int)_itemData.CostType].SetActive(true);
    }

    private void ChooseIncomeTypeObject()
    {
        foreach (GameObject item in incomeTypeImageTypes)
        {
            item.SetActive(false);
        }
        incomeTypeImageTypes[(int)_itemData.IncomeType].SetActive(true);
    }

    public void DeactivateView() 
    {
        _itemData.Count.Dispose();
        upgradeButton.onClick.RemoveAllListeners();
    }
}

public class ShopItemPanelService : IService
{
    [Inject] private IViewFabric _fabric;
    [Inject] private IMarkerService _markerService;
    [Inject] private ItemPoolDataManager _itemPoolDataManager;
    [Inject] private UpgradeDataManager _upgradeDataManager;
   
    private ShopItemPanelView _shopItemPanelView;
    private ItemData _itemData;
    private List<IDisposable> _disposables = new();
    public void ActivateService(ItemData itemData)
    {
        _itemData = itemData;
        Transform parent = _markerService.GetTransformMarker<ShopItemPosMarker>();
        _shopItemPanelView = _fabric.Init<ShopItemPanelView>(parent);
        _shopItemPanelView.OnUpgradeAction = OnUpgradeAction;
        _shopItemPanelView.ActivateView(_itemData);
       
    }

    public void ActivateService()
    {
        
    }

    private void OnUpgradeAction()
    {
        _upgradeDataManager.UpgradeItem(_itemData);
    }

    public void DeactivateService()
    {
        // Add your deactivation logic here
    }
}

public class StatsData
{
    public int CurrentLevel;
    public float BasicStatValue;
    public float CurrentStatValue { get => BasicStatValue + CurrentLevel * StatValueStep; }
    public float StatValueStep;
}

public class StatsDataManager
{
    public int MaxStatsLevel;
    public int BaseStatsCost;
    public int LevelCostStep;

    public void UpgradeStat(StatsData statsData)
    {
        if(statsData.CurrentLevel < MaxStatsLevel)
        {
            statsData.CurrentLevel++;
        }
    }
}