using UnityEngine;

public class TargetTakeDamageComponent : MonoBehaviour
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
        if (collision.TryGetComponent(out TargetDealDamageComponent comp))
        {
            if (comp.GetDealDamageType() == _type && comp.GetTargetTransfrom() == transform)
            {
                _hpComponent.TakeDamage(comp.GetDamageCount());
            }
        }
    }
}

