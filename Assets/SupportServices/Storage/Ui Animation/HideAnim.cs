using DG.Tweening;
using UnityEngine;

public class HideAnim : MonoBehaviour
{
    
    [SerializeField] private CanvasGroup _canvasGroup;
    private Sequence _sequence;
    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        _sequence?.Kill();
        _sequence = DOTween.Sequence().SetUpdate(true);
        _sequence.Append(_canvasGroup.DOFade(0,0.5f));
        _sequence.AppendInterval(0.5f);
        _sequence.Append(_canvasGroup.DOFade(1,0.5f));
        _sequence.AppendInterval(2f);
        _sequence.SetLoops(-1);
    }
}
