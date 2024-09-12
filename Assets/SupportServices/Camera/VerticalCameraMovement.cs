using UnityEngine;

public class VerticalCameraMovement : MonoBehaviour
{
    // Пороговое значение для определения свайпа
    public float swipeThreshold = 50f;

    // Переменные для отслеживания начальной и конечной позиций касания
    private Vector2 mouseDownPosition;
    private Vector2 mouseUpPosition;
    private Vector3 currentPos;

    private void Start()
    {
        currentPos = transform.position;
    }

    void Update()
    {
        // Проверка касания экрана или клика мыши
        if (Input.GetMouseButtonDown(0))
        {
            mouseDownPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            mouseUpPosition = Input.mousePosition;

            // Определение разницы между начальной и конечной позициями
            float swipeDeltaY = mouseUpPosition.y - mouseDownPosition.y;

            // Определение направления свайпа и вызов соответствующего события
            if (Mathf.Abs(swipeDeltaY) > swipeThreshold)
            {
                if (swipeDeltaY < 0)
                {
                    OnSwipeDown();
                }
                else
                {
                    OnSwipeUp();
                }
            }
        }
        
    }

    public void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentPos, Time.deltaTime / Time.timeScale * 25);
    }

    public void OnSwipeDown()
    {
        currentPos += Vector3.up * 5;
        currentPos.y = Mathf.Clamp(currentPos.y, -30f, 5f);
        Debug.Log("SwipeDown");
    }

    public void OnSwipeUp()
    {
        currentPos += Vector3.down * 5;
        currentPos.y = Mathf.Clamp(currentPos.y, -30f, 5f);
        Debug.Log("SwipeUp");
    }
}
