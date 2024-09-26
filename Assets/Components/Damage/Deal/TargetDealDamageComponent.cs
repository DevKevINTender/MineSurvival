using System;
using UnityEngine;

public class TargetDealDamageComponent : MonoBehaviour
{
    private Type _type;
    private float _dealDamageCount;
    private Transform _target;

    public void ActivateComponent(float dealDamageCount, Type type, Transform target)
    {
        _dealDamageCount = dealDamageCount;
        _type = type;
        _target = target;
    }

    public float GetDamageCount() => _dealDamageCount;
    public Type GetDealDamageType() => _type;
    public Transform GetTargetTransfrom() => _target;
}

