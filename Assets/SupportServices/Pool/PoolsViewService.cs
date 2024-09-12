using System;
using System.Collections.Generic;
using Zenject;

public interface IPoolsViewService
{
    public IPoolViewService GetPool<T>(int count = 1) where T : IPoolingViewService;
}

public class PoolsViewService : IPoolsViewService
{
    [Inject] private IServiceFabric _serviceFabric;
    private Dictionary<Type, IPoolViewService> _pools = new();

    public IPoolViewService GetPool<T>(int count = 1) where T : IPoolingViewService
    {
        return _pools.TryGetValue(typeof(T), out var pool) ? pool : InitPool<T>();
    }

    private IPoolViewService InitPool<T>(int count = 1) where T : IPoolingViewService
    {
        PoolViewService newPool = _serviceFabric.InitMultiple<PoolViewService>();
        newPool.ActivateService();
        newPool.SpawPool<T>(count);
        _pools.Add(typeof(T), newPool);
        return newPool;
    }
}
