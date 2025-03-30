using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;

public class SettingUI : MonoBehaviour
{
    [SerializeField] private Slider _mainSoundSlider, _bgmSoundSlider, _vfxSoundSlider;
    [SerializeField] private TextMeshProUGUI _mainSoundText, _bgmSoundText, _vfxSoundText;
    [SerializeField] private Button _homeBtn, _playBtn, _quitBtn;

    public RectTransform RectTrm {  get; private set; }

    private void Awake()
    {
        RectTrm = transform as RectTransform;
        _mainSoundSlider.value = AudioManager.Instance.volumeSaveData.allVolume;
        _bgmSoundSlider.value = AudioManager.Instance.volumeSaveData.bgmVolume;
        _vfxSoundSlider.value = AudioManager.Instance.volumeSaveData.sfxVolume;
        HandleSoundValueChangedEvent(0);

        _homeBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("TitleScene");
            UIManager.Instance.CloseSettingUI();
        });
        _playBtn.onClick.AddListener(UIManager.Instance.CloseSettingUI);
        _quitBtn.onClick.AddListener(Application.Quit);

        _mainSoundSlider.onValueChanged.AddListener(HandleSoundValueChangedEvent);
        _bgmSoundSlider.onValueChanged.AddListener(HandleSoundValueChangedEvent);
        _vfxSoundSlider.onValueChanged.AddListener(HandleSoundValueChangedEvent);
    }

    private void HandleSoundValueChangedEvent(float value)
    {
        _mainSoundText.text = Mathf.RoundToInt(_mainSoundSlider.value * 100).ToString();
        _bgmSoundText.text = Mathf.RoundToInt(_bgmSoundSlider.value * 100).ToString();
        _vfxSoundText.text = Mathf.RoundToInt(_vfxSoundSlider.value * 100).ToString();
        AudioManager.Instance.SetVolume(_mainSoundSlider.value, _bgmSoundSlider.value, _vfxSoundSlider.value);
    }

    public void SettingActive(bool value)
    {
        _mainSoundSlider.enabled = value;
        _bgmSoundSlider.enabled = value;
        _vfxSoundSlider.enabled = value;
    }
}
