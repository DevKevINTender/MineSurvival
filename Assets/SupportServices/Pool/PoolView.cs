
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
    public void SpawPool(Type objType, int objCount = 10);
    public IPoolingViewService GetItem();
    public IPoolingViewService GetItem<V>() where V : PoolingView;
    public void ReturnItem(IPoolingViewService item);
    public int GetViewServicesCount();
}

public class PoolViewService : IPoolViewService
{
    private List<IPoolingViewService> _freeItems = new();

    private List<IPoolingViewService> _viewServices = new();
    [Inject] private IServiceFabric _serviceFabric;
    [Inject] private IViewFabric _viewFabric;
    private PoolView _poolView;
	private int _objCount;
    private Type _serviceType;
    private Type _viewType;

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

    public IPoolingViewService GetItem<V>() where V : PoolingView
    {
        if (_freeItems.Count == 1) SpawnAddedItem<V>();
        IPoolingViewService Item = _freeItems.FirstOrDefault();
        _freeItems.Remove(Item);
        return Item;
    }


    public void ReturnItem(IPoolingViewService item)  
    {
        _freeItems.Add(item);
    }

    public void SpawPool(Type objType, int objCount = 10) 
    {
        _serviceType = objType;
        _objCount = objCount;
        _poolView.name = $"PoolView ({_serviceType.Name})";
        for (int i = 0; i < _objCount; i++)
        {
            SpawnAddedItem();
        }
    }

    public void SpawPool<S, V>(int objCount = 10) where V : PoolingView
    {
        _serviceType = typeof(S);
        _viewType = typeof(V);
        _objCount = objCount;
        _poolView.name = $"PoolView ({_serviceType.Name})";
        for (int i = 0; i < _objCount; i++)
        {
            SpawnAddedItem<V>();
        }
    }

    private void SpawnAddedItem() 
    {
        IPoolingViewService item = (IPoolingViewService)_serviceFabric.InitMultiple(_serviceType);
        item.ActivateServiceFromPool(_poolView.transform);
        item.SetDeactivateAction(ReturnItem);
        _viewServices.Add(item);
        _freeItems.Add(item);
    }

    private void SpawnAddedItem<V>() where V : PoolingView
    {
        IPoolingViewService item = (IPoolingViewService)_serviceFabric.InitMultiple(_serviceType);
        (item as PoolingViewService).ActivateServiceFromPool<V>(_poolView.transform);
        (item as PoolingViewService).SetDeactivateAction(ReturnItem);
        _viewServices.Add(item);
        _freeItems.Add(item);
    }
}
