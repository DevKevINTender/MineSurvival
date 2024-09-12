using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class HpComponent: MonoBehaviour
{
    public Action DieAction;
    private ReactiveProperty<float> hp = new ReactiveProperty<float>();
    public void ActivateComponent(float baseHp)
    {
        hp.Value = baseHp;
    }
    public void TakeDamage(float damage)
    {
        hp.Value -= damage;
        if(hp.Value <= 0)
        {
            DieAction?.Invoke();
        }
    }
}

