using System;
using System.Collections.Generic;
using UnityEngine;


public class OnContactComponent : MonoBehaviour
{
    public HashSet<System.Type> contactTypes = new();
    public Action hasContactAction;

    public void ActivateComponent()
    {

    }

    public void DeactivateComponent()
    {
        contactTypes.Clear();
    }

    public void Add(System.Type type)
    {
        contactTypes.Add(type);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (HasAnyComponentFromContactList(collision))
        {
            hasContactAction?.Invoke();
        }
    }

    private bool HasAnyComponentFromContactList(Collider2D collision)
    {
        foreach (var type in contactTypes)
        {

            var component = collision.GetComponent(type);


            if (component != null)
            {
                return true;
            }
        }

        return false;
    }
}


