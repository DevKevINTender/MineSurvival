using UnityEngine;

public class DealDamageComponent: MonoBehaviour
{
    private float _dealDamageCount;

    public void ActivateComponent(float dealDamageCount)
    {
        _dealDamageCount = dealDamageCount;
    }

    public float GetDamageCount() => _dealDamageCount;
    
}

