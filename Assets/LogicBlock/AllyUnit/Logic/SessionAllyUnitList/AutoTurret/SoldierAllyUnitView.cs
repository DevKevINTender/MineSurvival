using System;
using UniRx;
using UnityEngine;

public class SoldierAllyUnitView: AllyUnitView
{
    public Action OnLostTargetAction;
    public Transform gun;
    public Transform gunBarrel;
    public Transform currentTarget;
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

    public void Update()
    {
        if(currentTarget == null)
        {
            OnLostTargetAction?.Invoke();
        }
        else
        {
            gun.right = gun.transform.position - currentTarget.transform.position;
        }
    }

    public void SetTarget(Transform target)
    {
        currentTarget = target;
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

