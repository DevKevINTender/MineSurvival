using System;
using System.Collections.Generic;
using Zenject;

public interface IPoolsViewService
{
    public IPoolViewService GetPool<T>(int count = 1) where T : IPoolingViewService;
    public IPoolViewService GetPool<T, K>(int count = 1) where T : PoolingViewService where K : PoolingView;
}

public class PoolsViewService : IPoolsViewService
{
    private IServiceFabric _serviceFabric;
    private Dictionary<Type, IPoolViewService> _pools = new();

    [Inject]
    public void Constructor(IServiceFabric serviceFabric)
    {
        _serviceFabric = serviceFabric;
    }

    public IPoolViewService GetPool<T>(int count = 1) where T : IPoolingViewService
    {
        return _pools.TryGetValue(typeof(T), out var pool) ? pool : InitPool<T>();
    }

    private IPoolViewService InitPool<T>(int count = 1) where T : IPoolingViewService
    {
        PoolViewService newPool = _serviceFabric.InitMultiple<PoolViewService>();
        newPool.ActivateService();
        newPool.SpawPool(typeof(T), count);
        _pools.Add(typeof(T), newPool);
        return newPool;
    }

    public IPoolViewService GetPool<S, V>(int count = 1) where S : PoolingViewService where V : PoolingView
    {
        return _pools.TryGetValue(typeof(S), out var pool) ? pool : InitPool<S, V>();
    }

    private IPoolViewService InitPool<S, V>(int count = 1) where S : PoolingViewService where V : PoolingView
    {
        PoolViewService newPool = _serviceFabric.InitMultiple<PoolViewService>();
        newPool.ActivateService();
        newPool.SpawPool<S, V>(count);
        _pools.Add(typeof(S), newPool);
        return newPool;
    }
}
