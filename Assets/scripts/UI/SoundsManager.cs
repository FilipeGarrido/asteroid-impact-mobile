using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundsManager : MonoBehaviour
{
    [SerializeField] private UI UiCallBack;
    public TMP_Text MusicVolumeText, SoundFXVolumeText;
    public Slider musicVolumeSlider, soundEffectsVolumeSlider;
    public AudioSource GameMusic;
    public AudioSource[] SoundEffects;
    private readonly string FirstPlay = "FirstPlay";
    private static readonly string MusicPref = "MusicPref";
    private static readonly string SoundFxPref = "SoundFXPref";
    private float _musicFloat, _soundFxFloat;
 
    public void Awake()
    {
        int _firstPlay = PlayerPrefs.GetInt(FirstPlay);
        if(_firstPlay == 0){
            musicVolumeSlider.value = 0.65f;
            soundEffectsVolumeSlider.value = 0.8f;
            MusicVolumeText.text =  Mathf.Floor(musicVolumeSlider.value*100).ToString() + "%";
            SoundFXVolumeText.text =  Mathf.Floor(soundEffectsVolumeSlider.value*100).ToString() + "%";
            PlayerPrefs.SetFloat(MusicPref,musicVolumeSlider.value);
            PlayerPrefs.SetFloat(SoundFxPref,soundEffectsVolumeSlider.value);
            PlayerPrefs.SetInt(FirstPlay,1);
            LoadGame();
        }
        else{
            MusicVolumeText.text =  Mathf.Floor(musicVolumeSlider.value*100).ToString() + "%";
            SoundFXVolumeText.text =  Mathf.Floor(soundEffectsVolumeSlider.value*100).ToString() + "%";
            musicVolumeSlider.value =  PlayerPrefs.GetFloat(MusicPref);
            soundEffectsVolumeSlider.value = PlayerPrefs.GetFloat(SoundFxPref);
            LoadGame();
        }

    }

    public void ChangeVolume()
    {
        GameMusic.volume = musicVolumeSlider.value;
        for(int i=0; i< SoundEffects.Length ; i++){
            SoundEffects[i].volume = soundEffectsVolumeSlider.value;
        }
        MusicVolumeText.text =  Mathf.Floor(musicVolumeSlider.value*100).ToString() + "%";
        SoundFXVolumeText.text =  Mathf.Floor(soundEffectsVolumeSlider.value*100).ToString() + "%";
    }
     public void SaveGame(){
        PlayerPrefs.SetFloat(MusicPref,musicVolumeSlider.value);
        PlayerPrefs.SetFloat(SoundFxPref,soundEffectsVolumeSlider.value);
     }

    public void LoadGame()
    {   
        GameMusic.volume = musicVolumeSlider.value;
        for(int i=0; i< SoundEffects.Length ; i++){
            SoundEffects[i].volume = soundEffectsVolumeSlider.value;
        }
        UiCallBack.Back();
    }
}
