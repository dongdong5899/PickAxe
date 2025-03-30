using DG.Tweening;
using RPG.Players;
using System;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    public bool OnSettingUI {  get; private set; }
    [SerializeField] private PlayerInputSO _input;

    [Header("Prefab")]
    [SerializeField] private DamageFeedback _damageFeedback;

    [Header("UIs")]
    [SerializeField] private SettingUI _settingUI;
    [SerializeField] private ChangePannel _changePannel;
    [SerializeField] private GameOverPannel _gameOverPannel;
    [SerializeField] private GameObject _gameClearPannel;

    private Sequence _settingUISeq;

    private void Awake()
    {
        _input.ESCEvent += HandleESCEvent;
    }

    private void OnDestroy()
    {
        _input.ESCEvent -= HandleESCEvent;
        if (_settingUISeq != null && _settingUISeq.IsActive()) _settingUISeq.Kill();
    }

    public void OnGameOver()
    {
        _gameOverPannel.OnPannel();
    }
    public void OnGameClear()
    {
        _gameClearPannel.SetActive(true);
    }

    private void HandleESCEvent()
    {
        if (OnSettingUI) CloseSettingUI();
        else OpenSettingUI();
    }

    public Sequence OnScreenChangePannel(int stage)
    {
        return _changePannel.OnPannel(stage);
    }
    public Sequence OffScreenChangePannel()
    {
        return _changePannel.OffPannel();
    }

    public void CreateDamageFeedback(Vector3 pos, int damage, float spread = 0.5f)
    {
        DamageFeedback damageFeedback = Instantiate(_damageFeedback, pos, Quaternion.identity);
        damageFeedback.Init(damage, spread);
    }

    public void CloseSettingUI()
    {
        Time.timeScale = 1f;
        OnSettingUI = false;
        _input.PlayerActive(true);

        _settingUI.SettingActive(false);

        if (_settingUISeq != null && _settingUISeq.IsActive()) _settingUISeq.Kill();
        _settingUISeq = DOTween.Sequence();
        _settingUISeq.Append(_settingUI.RectTrm.DOAnchorPos(new Vector2(0, 1500), 0.2f).SetEase(Ease.OutCubic)).SetUpdate(true);
    }

    public void OpenSettingUI()
    {
        Time.timeScale = 0f; 
        OnSettingUI = true; 
        _input.PlayerActive(false);

        _settingUI.SettingActive(true);

        if (_settingUISeq != null && _settingUISeq.IsActive()) _settingUISeq.Kill();
        _settingUISeq = DOTween.Sequence();
        _settingUISeq.Append(_settingUI.RectTrm.DOAnchorPos(new Vector2(0, 0), 0.2f).SetEase(Ease.OutCirc)).SetUpdate(true);
    }
}
