using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

public interface IViewPool
{
    public IPoolingView GetItem();
    public void ReturnItem(IPoolingView item);
    public void SpawPool<T>(int objCount = 10);
}

public class ViewPool : IViewPool
{
    [Inject] private IViewFabric _viewFabric;

    private List<IPoolingView> _freeItems = new();
    private List<IPoolingView> _views = new();
    private ViewPoolSlot _viewPoolSlot;
    private int _objCount;
    private Type _viewType;

    public void ActivatePool()
    {
        _viewPoolSlot = _viewFabric.Init<ViewPoolSlot>();
    }

    public IPoolingView GetItem()
    {
        if (_freeItems.Count == 1) SpawnAddedItem();
        IPoolingView Item = _freeItems.FirstOrDefault();
        _freeItems.Remove(Item);
        return Item;
    }

    public void ReturnItem(IPoolingView item)
    {
        _freeItems.Add(item);
    }

    public void SpawPool<T>(int objCount = 10)
    {
        _objCount = objCount;
        _viewType = typeof(T);
        _viewPoolSlot.name = $"PoolView ({typeof(T).Name})";
        for (int i = 0; i < _objCount; i++)
        {
            SpawnAddedItem();
        }
    }

    private void SpawnAddedItem()
    {
        IPoolingView item = (IPoolingView)_viewFabric.Init(_viewType);
        item.ActivateViewFromPool(ReturnItem);
        _views.Add(item);
        _freeItems.Add(item);
    }
}


