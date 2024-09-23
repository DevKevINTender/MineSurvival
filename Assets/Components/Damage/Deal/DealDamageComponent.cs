﻿using UnityEngine;

public class DealDamageComponent: MonoBehaviour
{
    private DealDamageEnum _type;
    private float _dealDamageCount;

    public void ActivateComponent(float dealDamageCount, DealDamageEnum type)
    {
        _dealDamageCount = dealDamageCount;
        _type = type;
    }
    public float GetDamageCount() => _dealDamageCount;
    public DealDamageEnum GetDealDamageType() => _type;
    
}

public enum DealDamageEnum
{
    Ally,
    Enemy
}

