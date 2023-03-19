using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameCamera : MonoBehaviour
{
    private CinemachineVirtualCamera _cinemachineVCam;
    private CinemachineBasicMultiChannelPerlin _cinemachineBMCPerlin;
    private float _camShakeTimer;
    private float _camShakeTimerTotal;
    private float _camShakeStartingIntensity;

    [Header("Camera Shake Properties")]
    [SerializeField] private float _cameraShakeIntensity = 2f;
    [SerializeField] private float _cameraShakeDuration = 0.3f;

    private void OnEnable()
    {
        PlayerMovement.OnPlayerExploded += CallCameraShake;
    }

    private void OnDisable()
    {
        PlayerMovement.OnPlayerExploded -= CallCameraShake;
    }

    private void Awake()
    {
        _cinemachineVCam = GetComponent<CinemachineVirtualCamera>();
        _cinemachineBMCPerlin = _cinemachineVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Start()
    {
        CameraShake(_cameraShakeIntensity, _cameraShakeDuration);
    }

    // Update is called once per frame
    void Update()
    {
        if (_camShakeTimer > 0)
        {
            _camShakeTimer -= Time.deltaTime;
            _cinemachineBMCPerlin.m_AmplitudeGain = Mathf.Lerp(_camShakeStartingIntensity, 0f, 1 - (_camShakeTimer / _camShakeTimerTotal));
        }
    }

    private void CameraShake(float intensity, float duration)
    {
        _cinemachineBMCPerlin.m_AmplitudeGain = intensity;

        _camShakeStartingIntensity = intensity;
        _camShakeTimerTotal = duration;
        _camShakeTimer = duration;
    }

    private void CallCameraShake () => CameraShake(_cameraShakeIntensity, _cameraShakeDuration);
}
