using Zenject;
using UnityEngine;

public class ResetYGData : MonoBehaviour
{
    [ContextMenu("ResetData")]
    public void ResetAllData()
    {
        SaveLoader.ResetAll();
    }
}
