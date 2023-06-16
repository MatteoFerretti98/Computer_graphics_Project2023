using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAudioController : MonoBehaviour
{
    public Slider _musicSlider, _sfxSlider;
    public GameObject _popup;

    private void Start()
    {
        _musicSlider.value = PersistenceManager.PersistenceInstance.MusicValue*100;
        _sfxSlider.value = PersistenceManager.PersistenceInstance.FxValue*100;
        _popup.SetActive(false);
    }
    public void ToggleMusic()
    {
        AudioManager.instance.ToggleMusic();
    }

    public void ToggleSFX()
    {
        AudioManager.instance.ToggleSFX();
    }

    public void MusicVolume()
    {
        AudioManager.instance.MusicVolume(_musicSlider.value);
    }

    public void SFXVolume()
    {
        AudioManager.instance.SfXVolume(_sfxSlider.value);
    }

    public void HidePopup()
    {
        PersistenceManager.PersistenceInstance.FxValue = _sfxSlider.value/100;
        PersistenceManager.PersistenceInstance.MusicValue = _musicSlider.value / 100;
        PersistenceManager.PersistenceInstance.writeFile();
        _popup.SetActive(false);
    }
}
