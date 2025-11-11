using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] Slider _musicSlider;
    [SerializeField] AudioSource _audioSource;

    private void Start()
    {
        _musicSlider.value = _audioSource.volume;

        _musicSlider.onValueChanged.AddListener((value) =>
        {
            _audioSource.volume = value;
            
        });

    }

}
