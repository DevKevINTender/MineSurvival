using System;
using UnityEngine;

public class DealDamageComponent: MonoBehaviour, IDealDamageComponent
{
    private float _dealDamageCount;
    private DealDamageType _dealDamageType;

    public void ActivateComponent(float dealDamageCount, DealDamageType dealDamageType)
    {
        _dealDamageCount = dealDamageCount;
        _dealDamageType = dealDamageType;
    }
    public float GetDamageCount() => _dealDamageCount;
    public DealDamageType GetDealDamageType() => _dealDamageType;
    
    public void DeactivateComponent()
    {

    }
}

public enum DealDamageType
{
    Enemy,
    Ally
}

public enum DamageType
{

}

public interface IDealDamageComponent
{
    public DealDamageType GetDealDamageType();
    public float GetDamageCount();
}