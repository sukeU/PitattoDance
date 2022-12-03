using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderVolume : MonoBehaviour
{
    [SerializeField] string parameter;
    public Slider slider;
    public SoundManager soundManager;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        if (parameter == "Master")
        {
            slider.value = SoundManager.Instance.Volume;
        }else if (parameter == "Bgm")
        {
            slider.value = SoundManager.Instance.BgmVolume;
        }else if (parameter == "Se")
        {
            slider.value = SoundManager.Instance.SeVolume;
        }
        
    }
    public void VolumeSet()
    {
        SoundManager.Instance.Volume = slider.value;
    }

    public void BgmVolumeSet()
    {
        SoundManager.Instance.BgmVolume = slider.value;
    }
    public void SeVolumeSet()
    {

        //if (Mathf.Abs(SoundManager.Instance.SeVolume - slider.value) > 0.05f) { SoundManager.Instance.StopSe(); SoundManager.Instance.PlaySeByName("jump"); }
        SoundManager.Instance.SeVolume = slider.value;

    }

}

