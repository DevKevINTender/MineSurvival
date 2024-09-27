using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class HealTargetFinderComponent : MonoBehaviour
{
    public ReactiveProperty<TakeHealComponent> CurrentTarget = new();
    [SerializeField] private List<TakeHealComponent> targetPool = new();
    private DealHealType _healType;
    private bool isActive = false;
    public void ActivateComponent(DealHealType healType)
    {
        _healType = healType;
        isActive = true;
    }

    private void Update()
    {
        if (isActive) UpdateCurrentTarget();
    }

    public void DeactivateComponent()
    {
        isActive = false;

    }

    private void UpdateCurrentTarget()
    {
        if (targetPool.Count == 0)
        {
            CurrentTarget.Value = null;
            return;
        }

        TakeHealComponent prioritetTarget = null;
        float maxNeedHeal = 0;

        foreach (TakeHealComponent t in targetPool)
        {
            if (t.NeedHeal(out float count) && count > maxNeedHeal)
            {
                maxNeedHeal = count;
                prioritetTarget = t;
            }
        }

        // Обновляем CurrentTarget, если найдена новая ближайшая цель
        if (prioritetTarget != CurrentTarget.Value)
        {
            CurrentTarget.Value = prioritetTarget;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out TakeHealComponent component) && component.GetDealHealType() == _healType)
        {
            targetPool.Add(component);
            UpdateCurrentTarget();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out TakeHealComponent component) && component.GetDealHealType() == _healType)
        {
            targetPool.Remove(component);
            UpdateCurrentTarget();
        }
    }
}