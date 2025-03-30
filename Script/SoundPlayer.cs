using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    private AudioSource _audioSource;
    private float _startTime;
    private Sound _sound;

    public AudioClip currentAudioClip { get; private set; }

    public void Init(Sound sound)
    {
        _audioSource = GetComponent<AudioSource>();

        _sound = sound;
        AudioManager.Instance.OnVolumeChanged += HandleVolumeChanged;
        HandleVolumeChanged(AudioManager.Instance.volumeSaveData);

        float _3dValue = _sound.is3D ? 1.0f : 0.0f;
        _audioSource.spatialBlend = _3dValue;
        currentAudioClip = _sound.clip;

        if (_sound.duration == -1)
            _audioSource.loop = true;
        else
            _startTime = Time.time;

        _audioSource.clip = _sound.clip;
        _audioSource.Play();

        if (_sound.isDonDestroy)
            DontDestroyOnLoad(gameObject);
    }

    private void HandleVolumeChanged(VolumeSaveData data)
    {
        float volume = data.allVolume * _sound.volume;

        volume *= _sound.typeEnum == SoundType.BGM ?
            data.bgmVolume : data.sfxVolume;

        _audioSource.volume = volume;
    }

    private void Update()
    {
        if (_audioSource.loop) return;

        if (_startTime + _sound.duration < Time.time)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        AudioManager.Instance.OnVolumeChanged -= HandleVolumeChanged;
    }
}
