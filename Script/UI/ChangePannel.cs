using DG.Tweening;
using System.Linq;
using TMPro;
using UnityEngine;

public class ChangePannel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private Sequence _moveSeq;
    private RectTransform _rectTrm;

    private void Awake()
    {
        _rectTrm = transform as RectTransform;
    }

    public Sequence OnPannel(int stage)
    {
        if (stage == 6)
            _text.text = $"Boss";
        else
            _text.text = $"Stage {stage + 1}";
        _rectTrm.anchoredPosition = new Vector2(5000, 0);
        if (_moveSeq != null && _moveSeq.IsActive()) _moveSeq.Kill();
        _moveSeq = DOTween.Sequence();
        return _moveSeq.Append(_rectTrm.DOAnchorPosX(0, 1f).SetEase(Ease.OutCubic)).SetUpdate(true);
    }

    public Sequence OffPannel()
    {
        if (_moveSeq != null && _moveSeq.IsActive()) _moveSeq.Kill();
        _moveSeq = DOTween.Sequence();
        return _moveSeq.Append(_rectTrm.DOAnchorPosX(-5000, 1f).SetEase(Ease.InCubic)).SetUpdate(true);
    }
}
