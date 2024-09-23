using UnityEngine;
using Zenject;

public interface IViewPoolService
{
    public Transform GetPoolTransfrom();
}

public class ViewPoolService : IViewPoolService
{
    [Inject] private IViewFabric _viewFabric;
    private ViewPoolView _slotView;

    public Transform GetPoolTransfrom()
    {
        if(_slotView == null)
        {
            _slotView = _viewFabric.Init<ViewPoolView>();
        }
        return _slotView.transform;
    }

}
