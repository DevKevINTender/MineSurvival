using System;
using UniRx;
using UnityEngine;

public class ShieldHpComponent: MonoBehaviour, IHpComponent
{
    public Action CrashAction;
    public Action DieAction;
    private ReactiveProperty<float> _shieldPoints = new ReactiveProperty<float>();
    private ReactiveProperty<float> _healthPoints = new ReactiveProperty<float>();

    public void ActivateComponent(float hp, float shield)
    {
        _shieldPoints.Value = shield;
        _healthPoints.Value = hp;
    }

    public void TakeDamage(float damage)
    {
        _shieldPoints.Value -= damage;
        if(_shieldPoints.Value < 0)
        {
            CrashAction?.Invoke();
            var remainingDamage = 0 - _shieldPoints.Value;
            _healthPoints.Value -= remainingDamage;

            if (_healthPoints.Value <= 0)
            {
                DieAction?.Invoke();
            }
        }
    }
}