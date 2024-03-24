using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Instance { get; private set; }

    CinemachineFreeLook cinemachine;

    float shakeTimer;

    private void Awake()
    {
        Instance = this;
        cinemachine = GetComponent<CinemachineFreeLook>();
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0)
            {
                CinemachineBasicMultiChannelPerlin basicMultiChannelPerlin1 = cinemachine.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                CinemachineBasicMultiChannelPerlin basicMultiChannelPerlin2 = cinemachine.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                CinemachineBasicMultiChannelPerlin basicMultiChannelPerlin3 = cinemachine.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                basicMultiChannelPerlin1.m_AmplitudeGain = 0;
                basicMultiChannelPerlin2.m_AmplitudeGain = 0;
                basicMultiChannelPerlin3.m_AmplitudeGain = 0;
            }
        }
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin basicMultiChannelPerlin1 = cinemachine.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        CinemachineBasicMultiChannelPerlin basicMultiChannelPerlin2 = cinemachine.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        CinemachineBasicMultiChannelPerlin basicMultiChannelPerlin3 = cinemachine.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        basicMultiChannelPerlin1.m_AmplitudeGain = intensity;
        basicMultiChannelPerlin2.m_AmplitudeGain = intensity;
        basicMultiChannelPerlin3.m_AmplitudeGain = intensity;

        shakeTimer = time;
    }
}
