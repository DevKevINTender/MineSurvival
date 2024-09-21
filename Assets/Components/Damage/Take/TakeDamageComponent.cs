using System;
using UnityEngine;

public class TakeDamageComponent: MonoBehaviour
{
    private IHpComponent _hpComponent;
    private Type _compareType;

    public void ActivateComponent<T>() where T : DealDamageComponent
    {
        TryGetComponent(out _hpComponent);
        _compareType = typeof(T);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out DealDamageComponent comp) && comp.GetType() == _compareType)
        {
            _hpComponent.TakeDamage(comp.GetDamageCount());
        }
    }
}
