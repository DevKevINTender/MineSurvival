using System;
using UnityEngine;

public interface IPoolingViewService
{
    public void ActivateServiceFromPool(Action<IPoolingViewService> action, Transform poolTarget);
    public void DeactivateServiceToPool();
}
