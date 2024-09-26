using System;
using UnityEngine;
using Zenject;

public interface IViewFabric
{
    public T Init<T>();
    public T Init<T>(Vector3 position, Transform parent = null);
    public T Init<T>(Transform parent = null) where T : MonoBehaviour;
    public T Init<T>(T pb, Transform parent = null) where T : MonoBehaviour;
    public T Init<T>(GameObject pb, Transform parent = null);
    public T Init<T>(GameObject pb, Vector3 position, Transform parent = null);
    public Component Init(System.Type type, Transform parent = null);
}

public class ViewFabric : IViewFabric
{
    private IPrefabStorageService _prefabsStorageService;

    [Inject]
    public ViewFabric(IPrefabStorageService prefabsStorageService)
    {
        _prefabsStorageService = prefabsStorageService;
    }

    public T Init<T>()
    {
        var obj = MonoBehaviour.Instantiate(_prefabsStorageService.GetPrefabByType<T>(),
                                    Vector3.zero,
                                    Quaternion.identity);
        return obj.GetComponent<T>();
    }

    public T Init<T>(Vector3 position, Transform parent = null)
    {
        var obj = MonoBehaviour.Instantiate(_prefabsStorageService.GetPrefabByType<T>(),
                                    position,
                                    Quaternion.identity, parent);       
        return obj.GetComponent<T>();
    }

    public T Init<T>(Transform parent) where T : MonoBehaviour
    {       
        var obj = MonoBehaviour.Instantiate(_prefabsStorageService.GetPrefabByType<T>(), parent);
        return obj.GetComponent<T>();
    }

    public T Init<T>(GameObject pb, Transform parent = null)
    {
        var obj = MonoBehaviour.Instantiate(pb, parent);
        return obj.GetComponent<T>();
    }

    public T Init<T>(GameObject pb, Vector3 position, Transform parent = null)
    {
        var obj = MonoBehaviour.Instantiate(pb, parent);
        obj.transform.localPosition = position;
        return obj.GetComponent<T>();
    }

    public T Init<T>(T pb, Transform parent = null) where T : MonoBehaviour
    {
        T obj = MonoBehaviour.Instantiate(pb, parent);
        return obj.GetComponent<T>();
    }

    public Component Init(System.Type type, Transform parent = null)
    {
        var obj = MonoBehaviour.Instantiate(_prefabsStorageService.GetPrefabByType(type), parent);
        return obj.GetComponent(type);
    }
}


