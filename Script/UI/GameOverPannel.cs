using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPannel : MonoBehaviour
{
    [SerializeField] private Button _homeBtn, _quitBtn;

    private RectTransform _rectTrm;
    private Sequence _moveSeq;

    private void Awake()
    {
        _homeBtn.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("TitleScene");
        });
        _quitBtn.onClick.AddListener(Application.Quit);

        _rectTrm = transform as RectTransform;
    }

    public void OnPannel()
    {
        _rectTrm.anchoredPosition = new Vector2(0, 1500);
        if (_moveSeq != null && _moveSeq.IsActive()) _moveSeq.Kill();
        _moveSeq = DOTween.Sequence();
        _moveSeq.Append(_rectTrm.DOAnchorPosY(0, 1f).SetEase(Ease.OutCubic)).SetUpdate(true);
    }
}
