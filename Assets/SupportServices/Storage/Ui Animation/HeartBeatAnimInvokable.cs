using UnityEngine;
using DG.Tweening;

public class HeartBeatAnimInvokable : MonoBehaviour
{
    [SerializeField] private float _strength;
    [SerializeField] private float _duration;
    private Sequence _punchSequence;

    public void InvokeAnim()
    {
        _punchSequence?.Kill();
        _punchSequence = DOTween.Sequence().SetUpdate(true);
        _punchSequence.Append(transform.DOScale(Vector3.one * _strength, _duration / 2));
        _punchSequence.Append(transform.DOScale(Vector3.one, _duration / 2));
    }

    public void InvokeAnim(float strength, float duration)
    {
        _punchSequence?.Kill();
        _punchSequence = DOTween.Sequence().SetUpdate(true);
        _punchSequence.Append(transform.DOScale(Vector3.one * strength, duration / 2));
        _punchSequence.Append(transform.DOScale(Vector3.one, duration / 2));
    }

    public void OnDestroy()
    {
        _punchSequence?.Kill();
    }
}

