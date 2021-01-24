using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;


    public void SetMasterVolume(float volume)
    {
        _audioMixer.SetFloat("MasterVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        _audioMixer.SetFloat("SFXVolume", volume);

    }

    public void SetMusicVolume(float volume)
    {
        _audioMixer.SetFloat("MusicVolume", volume);

    }
}
