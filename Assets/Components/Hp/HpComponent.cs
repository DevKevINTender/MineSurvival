using System;
using UniRx;
using UnityEngine;

public interface IHpComponent
{
    public void TakeDamage(float damage);
}

public class HpComponent: MonoBehaviour, IHpComponent
{
    public Action DieAction;
    private ReactiveProperty<float> _healthPoints = new ReactiveProperty<float>();
    public void ActivateComponent(float baseHp)
    {
        _healthPoints.Value = baseHp;
    }
    public void TakeDamage(float damage)
    {
        _healthPoints.Value -= damage;
        Debug.Log(transform.name + " " + _healthPoints.Value);
        if(_healthPoints.Value <= 0)
        {
            DieAction?.Invoke();
        }
    }
}

