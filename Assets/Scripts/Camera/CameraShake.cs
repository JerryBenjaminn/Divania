using UnityEngine;
using System.Collections;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private float shakeElapsedTime;
    private float shakeDuration = 0f;
    private float shakeMagnitude = 0f;

    private void Awake()
    {
        if (virtualCamera == null)
        {
            virtualCamera = GameObject.FindWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>();
        }

        if (virtualCamera == null)
        {
            Debug.LogError("Virtual Camera not found. Please ensure it is in the scene and tagged as 'MainCamera'.");
        }
        else
        {
            Debug.Log("Virtual Camera found and assigned.");
        }
    }
    private void Update()
    {
        if (shakeElapsedTime > 0)
        {
            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = shakeMagnitude;
            shakeElapsedTime -= Time.deltaTime;
        }
        else
        {
            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;
        }
    }

    public void Shake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        shakeElapsedTime = shakeDuration;
    }
}


