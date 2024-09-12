using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

[RequireComponent(typeof(Button))]
public class ButtonAnimation : MonoBehaviour
{
    private Button button;
    private Sequence _punchSequence;

    public void Awake()
    {
        button = GetComponent<Button>();
    }

    public void PositiveAnim(Action endAction)
    {
        _punchSequence?.Kill();

        button.transform.localScale = Vector3.one;
        _punchSequence = DOTween.Sequence();
        _punchSequence.SetUpdate(true);
        _punchSequence.Append(button.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f, 2, 0));
        _punchSequence.OnComplete(() => endAction?.Invoke());
    }

    public void PositiveAnim()
    {
        _punchSequence?.Kill();

        button.transform.localScale = Vector3.one;
        _punchSequence = DOTween.Sequence();
        _punchSequence.SetUpdate(true);
        _punchSequence.Append(button.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f, 2, 0));
    }

    public void NegativeAnim()
    {
        _punchSequence?.Kill();

        Vector3 startPos = button.transform.localPosition;
        _punchSequence = DOTween.Sequence();
        _punchSequence.SetUpdate(true);
        _punchSequence.Append(button.GetComponent<RectTransform>().DOLocalMoveX(50, 0.10f));
        _punchSequence.Append(button.GetComponent<RectTransform>().DOLocalMoveX(-25f, 0.15f));
        _punchSequence.Append(button.GetComponent<RectTransform>().DOLocalMoveX(0f, 0.25f));
        _punchSequence.OnKill(() => button.transform.localPosition = startPos);

    }

    public void OnDestroy()
    {
        _punchSequence?.Kill();
    }

}
