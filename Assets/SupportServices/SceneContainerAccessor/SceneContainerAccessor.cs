using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class SceneContainerAccessor : MonoBehaviour
{
   private static DiContainer _sceneContainer;

    private void Awake()
    {   
        // Получаем текущий контекст сцены
        SceneContext sceneContext = FindObjectOfType<SceneContext>();
        if (sceneContext != null)
        {
            // Получаем контейнер Scene из контекста сцены
            _sceneContainer = sceneContext.Container;
        }
    }

    public static DiContainer SceneContainer
    {
        get { return _sceneContainer; }
    }
}