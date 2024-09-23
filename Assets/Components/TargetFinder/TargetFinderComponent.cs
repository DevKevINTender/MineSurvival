using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TargetFinderComponent: MonoBehaviour
{
    public Action<Transform> OnFindNewTargetAction;
    public ReactiveProperty<Transform> CurrentTarget = new();
    private List<Transform> targetPool = new();
    private Type _targetType;


    public void ActivateComponent(Type targetType)
    {
        _targetType = targetType;
    }

    public void Update()
    {
        if(CurrentTarget.Value == null)
        {
            float closestDistance = Mathf.Infinity;

            foreach (Transform t in targetPool)
            {
                float distance = Vector3.Distance(transform.position, t.position); // вычисление расстояния

                if (distance < closestDistance)
                {
                    closestDistance = distance; // обновление ближайшего расстояния
                    CurrentTarget.Value = t; // обновление ближайшего трансформа
                }
            }
        }
        else
        {
            CurrentTarget.Value = targetPool.Contains(CurrentTarget.Value) ? CurrentTarget.Value : null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_targetType != null)
        {
            var component = collision.GetComponent(_targetType);

            if (component is not null)
            {
                targetPool.Add(collision.transform);
            }
        }      
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_targetType != null)
        {
            var component = collision.GetComponent(_targetType);

            if (component != null)
            {
                targetPool.Remove(collision.transform);
            }
        }
    }
}

