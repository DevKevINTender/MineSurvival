using System;
using TMPro;
using UniRx;
using UnityEngine;

public interface IHpComponent
{
    public void TakeDamage(float damage);
}

public class HpComponent: MonoBehaviour, IHpComponent
{
    public TMP_Text HpText;
    public Action DieAction;
    public Action TakeDamageAction;
    private ReactiveProperty<float> _healthPoints = new ReactiveProperty<float>();
    public void ActivateComponent(float baseHp)
    {
        _healthPoints.Value = baseHp;
        _healthPoints.Subscribe(x => {

            if(HpText != null) HpText.text = x.ToString();

        }).AddTo(this);
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
}

