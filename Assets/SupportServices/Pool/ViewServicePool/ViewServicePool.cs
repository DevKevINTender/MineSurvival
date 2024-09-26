using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

public interface IViewServicePool
{
    public int GetViewServicesCount();
    public IPoolingViewService GetItem();
    public void ReturnItem(IPoolingViewService item);
    public void SpawPool<T>(int objCount = 10);
}

public class ViewServicePool : IViewServicePool
{
    [Inject] private IServiceFabric _serviceFabric;
    [Inject] private IViewFabric _viewFabric;

    private List<IPoolingViewService> _freeItems = new();
    private List<IPoolingViewService> _viewServices = new();
	private int _objCount;
    private System.Type _serviceType;

    public int GetViewServicesCount() => _viewServices.Count;

    public IPoolingViewService GetItem()
    {
        if (_freeItems.Count <= 0) SpawnAddedItem();
        IPoolingViewService Item = _freeItems.FirstOrDefault();
        _freeItems.Remove(Item);
        return Item;
    }

    public void ReturnItem(IPoolingViewService item)  
    {
        _freeItems.Add(item);
    }

    public void SpawPool<T>(int objCount = 10) 
    {
        _serviceType = typeof(T);
        _objCount = objCount;
        for (int i = 0; i < _objCount; i++)
        {
            SpawnAddedItem();
        }
    }

    private void SpawnAddedItem() 
    {
        IPoolingViewService item = (IPoolingViewService)_serviceFabric.InitMultiple(_serviceType);
        item.ActivateServiceFromPool(ReturnItem);
        _viewServices.Add(item);
        _freeItems.Add(item);
    }
}
