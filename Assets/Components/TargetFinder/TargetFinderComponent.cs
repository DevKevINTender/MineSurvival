using System;
using System.Collections.Generic;
using UnityEngine;

public class TargetFinderComponent: MonoBehaviour
{
    public Action<Transform> OnFindNewTargetAction;
    public Transform CurrentTarget = null;
    private Type _targetType;
    public List<Transform> targetPool = new();


    public void ActivateComponent(Type targetType)
    {
        _targetType = targetType;
    }

    public void Update()
    {
        if(CurrentTarget == null)
        {
            float closestDistance = Mathf.Infinity;

            foreach (Transform t in targetPool)
            {
                float distance = Vector3.Distance(transform.position, t.position); // вычисление расстояния

                if (distance < closestDistance)
                {
                    closestDistance = distance; // обновление ближайшего расстояния
                    CurrentTarget = t; // обновление ближайшего трансформа
                }
            }
        }
        else
        {
            CurrentTarget = targetPool.Contains(CurrentTarget) ? CurrentTarget : null;
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

