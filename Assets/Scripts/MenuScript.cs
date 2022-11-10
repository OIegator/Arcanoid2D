using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public static AudioSource audioSrc;

    [SerializeField]
    Toggle soundToggle;
    [SerializeField]
    Slider soundSlider;

    [SerializeField]
    Toggle musicToggle;
    [SerializeField]
    Slider musicSlider;

    public GameDataScript gameData;
    public GameObject quitButton;

    void Start()
    {
        audioSrc = Camera.main.GetComponent<AudioSource>();
        if (gameData.resetOnStart) {
            gameData.Load();
        }
        soundToggle.isOn = gameData.sound;
        soundSlider.value = gameData.soundVolume;
        musicToggle.isOn = gameData.music;
        musicSlider.value = gameData.musicVolume;
        if(PlayerScript.gameFinished)
        {
            quitButton.SetActive(true);
        }
        else
        {
            quitButton.SetActive(false);
        }
    }
    
    public void SetSoundVolume()
    {
        gameData.soundVolume = (int)soundSlider.value;
    }

    public void SetMusicVolume()
    {
        audioSrc.volume = musicSlider.value;
        gameData.musicVolume = audioSrc.volume;
    }

    public void PlayGame()
    {
        gameData.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void SetMusic()
    {
        if (musicToggle.isOn) 
        { 
            gameData.music = true;
            audioSrc.Play();
        }
        else 
        {
            gameData.music = false;
            audioSrc.Stop();
        }
    }

    public void SetSound()
    {
        if(soundToggle.isOn)
        {
            gameData.sound = true;
        }
        else
        {
            gameData.sound = false;
        }
    }

}
