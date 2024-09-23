using UnityEngine;

public class TargetDealDamageComponent : MonoBehaviour
{
    private DealDamageEnum _type;
    private float _dealDamageCount;
    private Transform _target;

    public void ActivateComponent(float dealDamageCount, Transform target)
    {
        _dealDamageCount = dealDamageCount;
        _target = target;
    }
    public void ActivateComponent(float dealDamageCount, DealDamageEnum type)
    {
        _dealDamageCount = dealDamageCount;
        _type = type;
    }
    public float GetDamageCount() => _dealDamageCount;
    public DealDamageEnum GetDealDamageType() => _type;
    public Transform GetTargetTransfrom() => _target;

}

