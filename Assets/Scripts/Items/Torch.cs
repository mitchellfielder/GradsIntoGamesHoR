using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : SupportItem
{
    private bool _isOn = false;
    [SerializeField]
    private GameObject _light;
    [SerializeField]
    private AudioClip _audioClipFlashlightOn;
    [SerializeField]
    private AudioClip _audioClipFlashlightOff;
    [SerializeField]
    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public override void Use(Player player)
    {
        if(!_light)
            return;
        _isOn = !_isOn;
        _light.SetActive(_isOn);
        player.GetMesh().GetComponent<Animator>().SetBool("Torch", _isOn);

        _audioSource.clip = (_isOn) ? _audioClipFlashlightOn : _audioClipFlashlightOff;
        _audioSource.Play();
    }
}
