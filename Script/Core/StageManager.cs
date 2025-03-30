using DG.Tweening;
using RPG.Players;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoSingleton<StageManager>
{
    [SerializeField] private List<GameObject> _stages;
    [SerializeField] private PlayerInputSO _input;
    private int _stageIdx = 0;

    private void Awake()
    {
        _stages.ForEach(stage => stage.SetActive(false));
        _stages[0].SetActive(true);
    }

    private void Start()
    {
        NextStage();
    }


    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    NextStage();
        //}
    }
    public void NextStage()
    {
        Time.timeScale = 0f;
        _input.UIActive(false);
        _input.PlayerActive(false);
        UIManager.Instance.OnScreenChangePannel(_stageIdx)
            .AppendCallback(() =>
            {
                if (_stageIdx >= 0)
                    _stages[_stageIdx].SetActive(false);
                _stageIdx++;
                _stages[_stageIdx].SetActive(true);
            })
            .AppendInterval(0.5f)
            .AppendCallback(() =>
            {
                UIManager.Instance.OffScreenChangePannel()
                .AppendCallback(() =>
                {
                    _input.UIActive(true);
                    _input.PlayerActive(true);
                    Time.timeScale = 1f;
                });
            });
        if (_stageIdx == 6)
        {
            //AudioManager.Instance.StopSound(SoundEnum.BGM, AudioManager.Instance.transform);
            //AudioManager.Instance.PlaySound(SoundEnum.BossBgm,transform);
        }
    }
}
