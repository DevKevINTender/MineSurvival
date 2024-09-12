using System;
using UnityEngine;

public class TakeDamageComponent: MonoBehaviour
{
    private HpComponent _hpComponent;
    private Type _compareType;

    public void ActivateComponent<T>() where T : DealDamageComponent
    {
        _hpComponent = GetComponent<HpComponent>();
        _compareType = typeof(T);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out DealDamageComponent comp))
        {
            if(comp.GetType() == _compareType)
            {
                _hpComponent.TakeDamage(comp.GetDamageCount());
            }
        }
    }
}

