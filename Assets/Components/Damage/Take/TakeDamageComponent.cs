﻿using System;
using UnityEngine;

public class TakeDamageComponent: MonoBehaviour
{
    private IHpComponent _hpComponent;
    private DealDamageEnum _type;

    public void ActivateComponent(DealDamageEnum type)
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

