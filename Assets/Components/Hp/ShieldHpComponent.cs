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
    public ReactiveProperty<float> _shieldPoints = new ReactiveProperty<float>();
    public ReactiveProperty<float> _healthPoints = new ReactiveProperty<float>();

    public void ActivateComponent(float hp, float shield)
    {
        _shieldPoints.Value = shield;
        _healthPoints.Value = hp;

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
            Debug.Log(transform.name + " hp" + _healthPoints.Value + " sp " + _shieldPoints.Value);
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
}