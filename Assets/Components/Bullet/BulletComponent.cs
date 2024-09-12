using UnityEngine;

public class BulletComponent : MonoBehaviour
{
    private Rigidbody2D rb;
    public void ActivateComponent(Vector3 dir)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = dir;
    }
}

