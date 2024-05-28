using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] Slider soundSlider;
    [SerializeField] AudioMixer soundMixer;

    [SerializeField] float defaultVolume;

    private void Start()
    {
        SetVolume(defaultVolume);
    }

    private void SetVolume(float value)
    {
        if (value < 1)
        {
            value = 0.01f; //Prevents zeroing errors
        }

        RefreshSlider(value);
        soundMixer.SetFloat("MasterVolume", MathF.Log10(value / 100) * 20f);
    }

    public void SetVolumeFromSlider()
    {
        SetVolume(soundSlider.value);
    }

    private void RefreshSlider(float value)
    {
        soundSlider.value = value;
    }
}
