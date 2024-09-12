using System;
using UnityEngine;

public interface IPoolingViewService
{
    public void ActivateServiceFromPool(Transform poolTarget);
    public void SetDeactivateAction(Action<IPoolingViewService> action);
}
