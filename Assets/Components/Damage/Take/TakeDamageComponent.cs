using System;
using UnityEngine;

public class TakeDamageComponent: MonoBehaviour
{
    private IHpComponent _hpComponent;
    private DealDamageType _type;

    public void ActivateComponent(DealDamageType type)
    {
        TryGetComponent(out _hpComponent);
        _type = type;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out DealDamageComponent comp))
        {
            if(comp.GetDealDamageType() == _type)
            {
                _hpComponent.TakeDamage(comp.GetDamageCount());
            }
        }
    }
}

