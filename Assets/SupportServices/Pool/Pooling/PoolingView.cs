using System;
using UnityEngine;

public interface IPoolingView
{
    public void ActivateViewFromPool(Action<IPoolingView> onDeativateViewToPoolAction);
    public void DeativateViewToPool();
}

public class PoolingView : MonoBehaviour, IPoolingView
{
    private Action<IPoolingView> _onDeativateViewToPoolAction;
    public void ActivateViewFromPool(Action<IPoolingView> onDeativateViewToPoolAction)
    {
        _onDeativateViewToPoolAction = onDeativateViewToPoolAction;
    }

    public void DeativateViewToPool()
    {
        _onDeativateViewToPoolAction(this);
    }
}
