using System;
using UnityEngine;

public class TargetTakeDamageComponent : MonoBehaviour
{
    private IHpComponent _hpComponent;
    private Type _type;

    public void ActivateComponent(Type type)
    {
        TryGetComponent(out _hpComponent);
        _type = type;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out TargetDealDamageComponent comp))
        {
            if (comp.GetDealDamageType() == _type && comp.GetTargetTransfrom() == transform)
            {
                _hpComponent.TakeDamage(comp.GetDamageCount());
            }
        }
    }
}

