using System;
using System.Collections.Generic;
using Zenject;

public interface IViewServicePoolService
{
    public IViewServicePool GetPool<T>(int count = 1) where T : IPoolingViewService;
}

public class ViewServicePoolService : IViewServicePoolService
{
    [Inject] private IServiceFabric _serviceFabric;
    private Dictionary<Type, IViewServicePool> _pools = new();

    public IViewServicePool GetPool<T>(int count = 1) where T : IPoolingViewService
    {
        return _pools.TryGetValue(typeof(T), out var pool) ? pool : InitPool<T>();
    }

    private IViewServicePool InitPool<T>(int count = 1) where T : IPoolingViewService
    {
        ViewServicePool newPool = _serviceFabric.InitMultiple<ViewServicePool>();
        newPool.SpawPool<T>(count);
        _pools.Add(typeof(T), newPool);
        return newPool;
    }
}

