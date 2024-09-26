using UniRx;
using UnityEngine;

public class AttackAllyUnitView: AllyUnitView
{
    public Transform gun;
    public Transform gunBarrel;
    public RangeProjectileView RangeProjectileViewPrefab;
    [SerializeField] private Animator _animator;
    [SerializeField] private TargetFinderComponent _targetFinderComponent;
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

    public void StartShoot()
    {
        _animator.SetBool("Shoot" , true);
        gun.right = gun.transform.position - _targetFinderComponent.CurrentTarget.Value.transform.position;
    }

    public void StopShoot()
    {
        _animator.SetBool("Shoot", false);
        gun.right = gun.transform.right;
    }

    public void TakeDamage()
    {
        _animator.Play("TakeDamage");
    }

    public void DeactivateView()
    {
        gameObject.SetActive(false);
    }
}

