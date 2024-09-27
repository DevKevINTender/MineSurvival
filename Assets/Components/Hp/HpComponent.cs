using System;
using TMPro;
using UniRx;
using UnityEngine;

public interface IHpComponent
{

    public void TakeDamage(float damage);
    public void TakeHeal(float heal);
    public bool NeedHeal(out float count);
}

public class HpComponent: MonoBehaviour, IHpComponent
{
    public TMP_Text HpText;
    public Action DieAction;
    public Action TakeDamageAction;
    private ReactiveProperty<float> _healthPoints = new();
    private float _maxHealthPoints;


    public void ActivateComponent(float baseHp)
    {
        _healthPoints.Value = baseHp;
        _maxHealthPoints = baseHp;
        _healthPoints.Subscribe(x => {

            if(HpText != null) HpText.text = x.ToString();

        }).AddTo(this);
    }

    public bool NeedHeal(out float count)
    {
        count = _maxHealthPoints - _healthPoints.Value;
        return count > 0;
    }

    public void TakeDamage(float damage)
    {
        _healthPoints.Value -= damage;
        TakeDamageAction?.Invoke();
        if (_healthPoints.Value <= 0)
        {
            DieAction?.Invoke();
        }
    }

    public void TakeHeal(float heal)
    {
        _healthPoints.Value += heal;
        if(_healthPoints.Value > _maxHealthPoints)
        {
            _healthPoints.Value = _maxHealthPoints;
        }
    }
}

