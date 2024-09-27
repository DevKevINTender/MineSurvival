using UniRx;
using UnityEngine;

public class SupportAllyUnitView : AllyUnitView
{
    public HealProjectileView HealProjectileViewPreafab;
    [SerializeField] private Animator _animator;
    public HealTargetFinderComponent TargetFinderComponent;
    public HpComponent HpComponent;
    public TakeDamageComponent TakeDamageComponent;
    public TakeHealComponent TakeHealComponent;

    private ReactiveProperty<SkillStatus> _currentStatus;
    private AllyUnitData _skillData;


    public void ActivateView(ReactiveProperty<SkillStatus> status, AllyUnitData skillData)
    {
        _skillData = skillData;
        _currentStatus = status;
        _currentStatus
           .Subscribe(x =>
           {
               switch (x)
               {
                   case SkillStatus.Active:
                       //дроп на поле и начать стрельбу
                       break;
                   case SkillStatus.Recovery:
                       // Востановление
                       break;
                   case SkillStatus.ReadyToActive:
                       // подсветить площадку
                       break;
               }
           })
           .AddTo(this);
    }

    public void StartHeal()
    {
        _animator.SetBool("Heal", true);
    }

    public void StopHeal()
    {
        _animator.SetBool("Heal", false);
    }

    public void TakeDamage()
    {
        _animator.Play("TakeDamage");
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
