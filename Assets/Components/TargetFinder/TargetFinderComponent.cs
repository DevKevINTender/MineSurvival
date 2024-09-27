using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TargetFinderComponent : MonoBehaviour
{
    public ReactiveProperty<Transform> CurrentTarget = new();
    private List<Transform> targetPool = new();
    private Type _targetType;
    private bool isActive = false;
    public void ActivateComponent(Type targetType)
    {
        _targetType = targetType;
        isActive = true;
    }

    private void Update()
    {
       if(isActive) UpdateCurrentTarget();
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

        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform t in targetPool)
        {
            float distance = Vector3.Distance(transform.position, t.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = t;
            }
        }

        // Обновляем CurrentTarget, если найдена новая ближайшая цель
        if (closestTarget != CurrentTarget.Value)
        {
            CurrentTarget.Value = closestTarget;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_targetType != null && collision.TryGetComponent(_targetType, out var component))
        {
            targetPool.Add(collision.transform);
            // Проверяем, если это новая цель, чтобы сразу назначить ее
            UpdateCurrentTarget();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_targetType != null && collision.TryGetComponent(_targetType, out var component))
        {
            targetPool.Remove(collision.transform);
            UpdateCurrentTarget(); // Обновляем текущую цель при выходе
        }
    }
}
