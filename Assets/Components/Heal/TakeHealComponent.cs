using UnityEngine;

public class TakeHealComponent : MonoBehaviour
{
    private IHpComponent _hpComponent;
    private DealHealType _type;

    public void ActivateComponent(DealHealType type)
    {
        TryGetComponent(out _hpComponent);
        _type = type;
        
    }

    public DealHealType GetDealHealType() => _type;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out DealHealComponent comp))
        {
            if (comp.GetDealHealType() == _type)
            {
                _hpComponent.TakeHeal(comp.GetHealCount());
            }
        }
    }

    public bool NeedHeal(out float count)
    {
        bool result = _hpComponent.NeedHeal(out float needHp);
        count = needHp;
        return result;
    }
}
