using System;
using UnityEngine;

public class EnemyTargetFinderComponent: MonoBehaviour
{
    public Action<EnemyView> OnFindNewTargetAction;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyView enemy))
        {
            OnFindNewTargetAction?.Invoke(enemy);
        }
    }
}

