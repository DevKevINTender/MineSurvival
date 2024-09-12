using UnityEngine;
using DG.Tweening;

public class HeartBeatAnimC : MonoBehaviour
{
    [SerializeField] private float _strength;
    [SerializeField] private float _duration;
    private Sequence _punchSequence;

    public void Awake()
    {
        _punchSequence?.Kill();
        _punchSequence = DOTween.Sequence().SetUpdate(true);
        _punchSequence.Append(transform.DOScale(Vector3.one * _strength, _duration / 2));
        _punchSequence.Append(transform.DOScale(Vector3.one, _duration / 2));
        _punchSequence.SetLoops(-1);
    }

    public void OnDestroy()
    {
        _punchSequence?.Kill();
    }
}

