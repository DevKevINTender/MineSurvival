using System;
using TMPro;
using UniRx;
using UnityEngine;

public class ShieldHpComponent: MonoBehaviour, IHpComponent
{
    public TMP_Text HpText;
    public TMP_Text SpText;
    public Action CrashAction;
    public Action DieAction;
    public Action TakeDamageAction;
    private ReactiveProperty<float> _shieldPoints = new ReactiveProperty<float>();
    private ReactiveProperty<float> _healthPoints = new ReactiveProperty<float>();
    private float _maxHealthPoints;

    public void ActivateComponent(float hp, float shield)
    {
        _shieldPoints.Value = shield;
        _healthPoints.Value = hp;
        _maxHealthPoints = hp;

        _healthPoints.Subscribe(x => {
            HpText.text = x.ToString();
        }).AddTo(this);

        _shieldPoints.Subscribe(x => {
                SpText.text = x.ToString();
        }).AddTo(this);
    }

    public void TakeDamage(float damage)
    {
        TakeDamageAction?.Invoke();


        if (_shieldPoints.Value <= 0)
        {

            _healthPoints.Value -= damage;
            if (_healthPoints.Value <= 0)
            {
                DieAction?.Invoke();
            }
        }
        else
        {
            _shieldPoints.Value -= damage;
            if (_shieldPoints.Value <= 0)
            {
                _shieldPoints.Value = 0;
                CrashAction?.Invoke();
            }
        }


    }

    public bool NeedHeal(out float count)
    {
        count = _maxHealthPoints - _healthPoints.Value;
        return count > 0;
    }

    public void TakeHeal(float heal)
    {
        _healthPoints.Value += heal;
        Debug.Log("Take Heal" + heal);

        if (_healthPoints.Value > _maxHealthPoints)
        {
            _healthPoints.Value = _maxHealthPoints;
        }
    }
}