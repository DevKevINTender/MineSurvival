
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class PoolView: MonoBehaviour
{

}

public interface IPoolViewService
{
    public void SpawPool<T>(int objCount = 10);
    public IPoolingViewService GetItem();
    public void ReturnItem(IPoolingViewService item);
    public int GetViewServicesCount();
}

public class PoolViewService : IPoolViewService
{
    [Inject] private IServiceFabric _serviceFabric;
    [Inject] private IViewFabric _viewFabric;

    private List<IPoolingViewService> _freeItems = new();
    private List<IPoolingViewService> _viewServices = new();
    private PoolView _poolView;
	private int _objCount;
    private Type _serviceType;

    public void ActivateService()
    {
        _poolView = _viewFabric.Init<PoolView>();       
    }

    public int GetViewServicesCount() => _viewServices.Count;

    public IPoolingViewService GetItem()
    {
        if (_freeItems.Count == 1) SpawnAddedItem();
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
        _poolView.name = $"PoolView ({_serviceType.Name})";
        for (int i = 0; i < _objCount; i++)
        {
            SpawnAddedItem();
        }
    }

    private void SpawnAddedItem() 
    {
        IPoolingViewService item = (IPoolingViewService)_serviceFabric.InitMultiple(_serviceType);
        item.ActivateServiceFromPool(ReturnItem, _poolView.transform);
        _viewServices.Add(item);
        _freeItems.Add(item);
    }
}
