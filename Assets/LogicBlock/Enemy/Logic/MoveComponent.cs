using UnityEngine;
using System;
using System.Collections;

public class MoveComponent: MonoBehaviour
{
    public Action AchiveTargetAction;
    public Action LooseTargetAcion;

    public void MoveToTarget(Transform target, float speed, float stopDistance)
    {
        StopAllCoroutines();
        StartCoroutine(MoveToTargetCoroutine(target, speed, stopDistance));
    }

    private IEnumerator MoveToTargetCoroutine(Transform target, float speed, float stopDistance)
    {
        while (target && Vector3.Distance(transform.position, target.position) > stopDistance)
        {
            // Вычисляем направление к цели
            Vector3 direction = (target.position - transform.position).normalized;
            // Перемещаем объект
            transform.position += direction * speed * Time.deltaTime;

            // Ждем следующий кадр
            yield return null;
        }

        if(target)
        {
           if(stopDistance <= 0.1f) transform.position = target.position;
            AchiveTargetAction?.Invoke();
        }
        else
        {
            LooseTargetAcion?.Invoke();
        }
    }

    public void DeactivateComponent()
    {
        StopAllCoroutines();
    }
}



