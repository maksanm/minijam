using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
	public AudioMixer am;
    public float sliderValue;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void AudioVolume()
    {
        sliderValue = GameObject.Find("Slider").GetComponent<Slider>().value;
        Debug.Log(sliderValue);
        am.SetFloat("masterVolume", sliderValue);
    }
}
