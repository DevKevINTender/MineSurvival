using UnityEngine;
using DG.Tweening;

public class HeartBeatAnim : MonoBehaviour
{
    private Sequence _punchSequence;

    public void Awake()
    {
        _punchSequence?.Kill();
        _punchSequence = DOTween.Sequence().SetUpdate(true);
        _punchSequence.Append(transform.DOPunchScale(Vector3.one * 0.2f, 0.2f, 2, 0));
        _punchSequence.SetLoops(-1);
    }


    public void OnDestroy()
    {
        _punchSequence?.Kill();
    }
}

