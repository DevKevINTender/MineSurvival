using System;
using System.Collections.Generic;
using UnityEngine;


public class OnContactComponent : MonoBehaviour
{
    public HashSet<Type> contactTypes = new();
    public Action hasContactAction;

    public void Add(Type type)
    {
        contactTypes.Add(type);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (HasAnyComponentFromContactList(collision))
        {
            hasContactAction?.Invoke();
            Debug.Log("BULLET");
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


