using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    [SerializeField] private Button _gameStartBtn, _quitBtn;

    private void Awake()
    {
        _gameStartBtn.onClick.AddListener(() => SceneManager.LoadScene("GameScene"));
        _quitBtn.onClick.AddListener(Application.Quit);
        //AudioManager.Instance.PlaySound(SoundEnum.TitleBgm, transform);
    }
}
