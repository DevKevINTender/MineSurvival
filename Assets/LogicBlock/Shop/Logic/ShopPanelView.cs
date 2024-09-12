using Zenject;
using UnityEngine;
using System.Collections.Generic;

public class ShopPanelView : MonoBehaviour
{

}

public class ShopPanelService : IService
{
    [Inject] private IViewFabric _viewFabric;
    [Inject] private IMarkerService _markerService;
	private ShopPanelView _ShopPanelView;

   
	public void ActivateService()
	{
        Transform parent = _markerService.GetTransformMarker<ShopPanelPosMarker>();
        _ShopPanelView = _viewFabric.Init<ShopPanelView>(parent);
    }
    
    public void DeactivateService()
    {

    }
}
