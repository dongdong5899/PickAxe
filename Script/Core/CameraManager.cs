using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
    private CinemachineVirtualCameraBase _currentCamera;
    private CinemachineBasicMultiChannelPerlin _currentMultiChannel;

    private Sequence _shakeSequence;

    private void Start()
    {
        SettingCurrentCamera();
    }

    private void SettingCurrentCamera()
    {
        CinemachineVirtualCameraBase currentCam = CinemachineCore.GetVirtualCamera(0);

        _currentCamera = currentCam;
        _currentMultiChannel = _currentCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ChangeCamera(CinemachineCamera camera)
    {
        _currentCamera.Priority = 10;
        _currentCamera = camera;
        _currentCamera.Priority = 11;
    }

    public void ShakeCamera(float amplitude, float frequency, float time, AnimationCurve curve)
    {
        if (_shakeSequence != null && _shakeSequence.IsActive()) _shakeSequence.Kill();
        _shakeSequence = DOTween.Sequence();

        _shakeSequence
            .Append(
                DOTween.To(() => amplitude,
                value => _currentMultiChannel.AmplitudeGain = value,
                0, time).SetEase(curve))
            .Join(
                DOTween.To(() => frequency,
                value => _currentMultiChannel.FrequencyGain = value,
                0, time).SetEase(curve));
    }
    public void ShakeCamera(float amplitude, float frequency, float time, Ease ease = Ease.Linear)
    {
        if (_shakeSequence != null && _shakeSequence.IsActive()) _shakeSequence.Kill();
        _shakeSequence = DOTween.Sequence();

        _shakeSequence
            .Append(
                DOTween.To(() => amplitude,
                value => _currentMultiChannel.AmplitudeGain = value,
                0, time).SetEase(ease))
            .Join(
                DOTween.To(() => frequency,
                value => _currentMultiChannel.FrequencyGain = value,
                0, time).SetEase(ease));
    }
}
