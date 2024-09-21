using System;
using System.Collections.Generic;
using Zenject;

public interface IViewPoolService
{
    public IViewPool GetPool<T>(int count = 1) where T : IPoolingView;
}


public class ViewPoolService : IViewPoolService
{
    [Inject] private IServiceFabric _serviceFabric;
    private Dictionary<Type, IViewPool> _pools = new();

    public IViewPool GetPool<T>(int count = 1) where T : IPoolingView
    {
        return _pools.TryGetValue(typeof(T), out var pool) ? pool : InitPool<T>();
    }

    private IViewPool InitPool<T>(int count = 1)
    {
        ViewPool newPool = _serviceFabric.InitMultiple<ViewPool>();
        newPool.ActivatePool();
        newPool.SpawPool<T>(count);
        _pools.Add(typeof(T), newPool);
        return newPool;
    }
}
