using UnityEngine;

public class ResizeCamera : MonoBehaviour
{
    // Задайте желаемую ширину в пикселях для вашего дизайна
    public float targetWidth = 1080f;

    void Start()
    {
        // Получаем текущий размер экрана
        float screenHeight = Screen.height;
        float screenWidth = Screen.width;

        // Рассчитываем новый размер камеры, учитывая желаемую ширину
        float targetHeight = targetWidth / screenWidth * screenHeight;
        float resiltSize = targetHeight / 200f; ;
        Camera.main.orthographicSize = resiltSize < 10f ? 10f : resiltSize;

        // Если вы используете perspective камеру, расскомментируйте следующую строку:
        // Camera.main.fieldOfView = 2f * Mathf.Atan(targetHeight / (2f * Camera.main.transform.position.z)) * Mathf.Rad2Deg;
    }
}
