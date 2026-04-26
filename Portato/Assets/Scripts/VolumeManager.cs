using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    void Start()
    {
        
    }

    public void SetVolume()
    {
        AudioListener.volume = volumeSlider.value;
    }


    
}
