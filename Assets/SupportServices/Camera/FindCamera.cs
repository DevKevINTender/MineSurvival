using UnityEngine;

public class FindCamera : MonoBehaviour
{
    private void Awake()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = FindAnyObjectByType<Camera>();
    }
}
