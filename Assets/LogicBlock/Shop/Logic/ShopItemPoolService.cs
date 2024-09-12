using System.Collections;
using System.Collections.Generic;
using Zenject;

public class ShopItemPoolService
{
    [Inject] private IServiceFabric _serviceFabric;
    [Inject] private ItemPoolDataManager _itemPoolDataManager;
    private List<ShopItemPanelService> _itemPanelServices = new();

    public void ActivateService()
    {
        foreach (var item in _itemPoolDataManager.GetItemList())
        {
            ShopItemPanelService newItem = _serviceFabric.InitMultiple<ShopItemPanelService>();
            _itemPanelServices.Add(newItem);
            newItem.ActivateService(item);
        }
    }

    public void DeactivateService()
    {

    }
}
