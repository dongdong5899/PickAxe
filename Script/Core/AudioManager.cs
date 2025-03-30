using Doryu.JBSave;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum SoundEnum
{
    BGM,

    PlayerWalk,
    PlayerAttack,
    PlayerParry,
    PlayerPull,
    PlayerDash,
    PlayerHitted,

    NearAttack,
    ShootAttack,
    InductionAttack,
    EnemyHitted,

    BossFist,
    BossHandBreak,

    UIChoose,
    BossBgm,
    TitleBgm,
    PlayerThorow
}
[System.Serializable]
public enum SoundType
{
    BGM,
    SFX
}

[System.Serializable]
public class Sound
{
    public SoundEnum nameEnum;
    public SoundType typeEnum;
    public float duration;
    [Range(0f, 1f)]
    public float volume = 0.5f;
    public bool is3D = true;
    public bool isDonDestroy;
    public AudioClip clip;
}

public class VolumeSaveData : ISavable<VolumeSaveData>
{
    public float allVolume = 1f;
    public float bgmVolume = 0.5f;
    public float sfxVolume = 0.5f;

    public void OnLoadData(VolumeSaveData classData)
    {
        allVolume = classData.allVolume;
        bgmVolume = classData.bgmVolume;
        sfxVolume = classData.sfxVolume;
    }

    public void OnSaveData(string savedFileName)
    {

    }

    public void ResetData()
    {

    }
}

public class AudioManager : MonoSingleton<AudioManager>
{
    [SerializeField] private Sound[] _sounds;

    [SerializeField] private SoundPlayer _soundPlayerPrefab;

    public VolumeSaveData volumeSaveData { get; private set; } = new VolumeSaveData();
    private Dictionary<SoundEnum, Sound> _soundDict = new Dictionary<SoundEnum, Sound>();

    public event Action<VolumeSaveData> OnVolumeChanged;

    private void Awake()
    {
        if (Instance != this) Destroy(gameObject);


        volumeSaveData.LoadJson("SoundVolume");


        foreach (Sound sound in _sounds)
        {
            _soundDict.Add(sound.nameEnum, sound);
        }
        DontDestroyOnLoad(gameObject);

        //PlaySound(SoundEnum.BGM, transform);
    }

    public void SetVolume(float all, float bgm, float sfx)
    {
        bool flag = false;
        if (volumeSaveData.allVolume != all)
        {
            volumeSaveData.allVolume = all;
            flag = true;
        }
        if (volumeSaveData.bgmVolume != bgm)
        {
            volumeSaveData.bgmVolume = bgm;
            flag = true;
        }
        if (volumeSaveData.sfxVolume != sfx)
        {
            volumeSaveData.sfxVolume = sfx;
            flag = true;
        }

        if (flag)
        {
            OnVolumeChanged?.Invoke(volumeSaveData);
            volumeSaveData.SaveJson("SoundVolume");
        }
    }

    public void PlaySound(SoundEnum soundEnum, Transform parent)
    {
        SoundPlayer soundPlayer = Instantiate(_soundPlayerPrefab, parent);
        soundPlayer.transform.localPosition = Vector3.zero;
        soundPlayer.Init(_soundDict[soundEnum]);
    }
    public void PlaySound(SoundEnum soundEnum, Vector3 pos)
    {
        SoundPlayer soundPlayer = Instantiate(_soundPlayerPrefab);
        soundPlayer.transform.position = pos;
        soundPlayer.Init(_soundDict[soundEnum]);
    }

    public void StopSound(SoundEnum soundEnum, Transform target)
    {
        SoundPlayer[] soundPlayers = target.GetComponentsInChildren<SoundPlayer>();

        Sound sound = _soundDict[soundEnum];

        for (int i = 0; i < soundPlayers.Length; i++)
        {
            if (soundPlayers[i].currentAudioClip == sound.clip)
            {
                soundPlayers[i].Die();
                return;
            }
        }
    }
}
