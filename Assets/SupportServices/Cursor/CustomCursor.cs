using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    private Vector3 cursorPosition;

    private Animation anim;

    void Start()
    {
        anim = GetComponentInChildren<Animation>();
#if UNITY_EDITOR
        Cursor.visible = false; // —крываем курсор мыши в редакторе
#endif
    }

    void Update()
    {
        if(Camera.main)
        {
            // ѕолучаем текущую позицию курсора в мировых координатах
            cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // ”станавливаем позицию спрайта равной позиции курсора, но оставл€ем z координату прежней
            transform.position = new Vector3(cursorPosition.x, cursorPosition.y, transform.position.z);

            if (Input.GetMouseButtonDown(0))
            {
                anim?.Play("Click"); // «апускаем анимацию
            }
        }
    }
}
