using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public static AudioSource audioSrc;

    // Поля привязки скрипта к слайдеру и чекбоксу для звука
    [SerializeField]
    Toggle soundToggle;
    [SerializeField]
    Slider soundSlider;

    // Поля привязки скрипта к слайдеру и чекбоксу для музыки
    [SerializeField]
    Toggle musicToggle;
    [SerializeField]
    Slider musicSlider;

    public GameDataScript gameData;

    // Поле привязки кнопки выхода из игры 
    public GameObject quitButton;

    void Start()
    {
        // Загрузка gameData на старте экрана с меню 
        audioSrc = Camera.main.GetComponent<AudioSource>();
        if (gameData.resetOnStart) {
            gameData.Load();
        }
        // При загрузки стартового меню значения слайдеров и чекбоксов беруться из памяти gameData
        soundToggle.isOn = gameData.sound;
        soundSlider.value = gameData.soundVolume;
        musicToggle.isOn = gameData.music;
        musicSlider.value = gameData.musicVolume;

        // Проверка активности флага конца игры, оборажающая кнопку выхода на стартовом меню
        if(PlayerScript.gameFinished)
        {
            quitButton.SetActive(true);
        }
        else
        {
            quitButton.SetActive(false);
        }
    }

    // Обработчик слайдера со звуком для записи значения в gameData
    public void SetSoundVolume()
    {
        gameData.soundVolume = (int)soundSlider.value;
    }

    // Обработчик слайдера с музыкой для записи значения в gameData
    public void SetMusicVolume()
    {
        audioSrc.volume = musicSlider.value;
        gameData.musicVolume = audioSrc.volume;
    }

    // Обработчик кнопки начала игры
    public void PlayGame()
    {
        gameData.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Обработчик кнопки выхода из игры
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // Обработчик чекбокса с музыкой для записи значения в gameData
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

    // Обработчик чекбокса со звуком для записи значения в gameData
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
