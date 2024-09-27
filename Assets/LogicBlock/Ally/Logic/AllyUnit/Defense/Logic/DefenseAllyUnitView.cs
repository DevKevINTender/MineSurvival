using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class DefenseAllyUnitView : AllyUnitView
{
    public Transform gun;
    public Transform gunBarrel;
    public MeleeProjectileView MeleeProjectilePrefab;
    [SerializeField] private Animator _animator;
    [SerializeField] private TargetFinderComponent _targetFinderComponent;
    private ReactiveProperty<SkillStatus> _currentStatus;
    private AllyUnitData _skillData;
    private SpriteRenderer _swordSprite;
    private List<Sprite> _swordSpriteTierList = new();
    private SpriteRenderer _shieldSprite;
    private List<Sprite> _shieldSpriteTierList = new();
    public TakeHealComponent TakeHealComponent;
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
        _animator.SetBool("Attack", true);
    }

    public void StopShoot()
    {
        _animator.SetBool("Attack", false);
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
