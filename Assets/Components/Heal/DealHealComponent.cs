using UnityEngine;
public interface IDealHealComponent
{
    public DealHealType GetDealHealType();
    public float GetHealCount();
}
public class DealHealComponent : MonoBehaviour, IDealHealComponent
{
    private float _dealHealCount;
    private DealHealType _dealHealType;

    public void ActivateComponent(float dealHealCount, DealHealType dealHealType)
    {
        _dealHealCount = dealHealCount;
        _dealHealType = dealHealType;
    }
    public float GetHealCount() => _dealHealCount;
    public DealHealType GetDealHealType() => _dealHealType;

    public void DeactivateComponent()
    {

    }
}

public enum DealHealType
{
    Enemy,
    Ally
}
