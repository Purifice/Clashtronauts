using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PauseMenu : MonoBehaviour
{

    public GameObject pauseContainer;
    public GameObject optionsContainer;
    public GameObject audioContainer;
    public PlayerMovement playermovement;
    public bool isPaused;
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    private SoundMixerManager soundMixerManager;

    private void Start()
    {
        isPaused = false;

        soundMixerManager = FindObjectOfType<SoundMixerManager>();

        masterVolumeSlider.onValueChanged.AddListener(soundMixerManager.SetMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(soundMixerManager.SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(soundMixerManager.SetSFXVolume);
    }
    void Update()
    {
        if (playermovement.menuAction && !isPaused && Time.timeScale != 0)
        {
           // Cursor.lockState = CursorLockMode.None;
            pauseContainer.SetActive(true);
            Time.timeScale = 0;
            isPaused = true;

        }
        else if (playermovement.menuAction && isPaused)
        {
            ResumeButton();
        }
        playermovement.menuAction = false;
    }
    
    public void ResumeButton()
    {
        //Debug.Log("Resume button clicked!");
        pauseContainer.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    public void SettingsButton()
    {
        pauseContainer.SetActive(false);
        optionsContainer.SetActive(true);
    }

    public void RestartButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void AudioButton()
    {
        optionsContainer.SetActive(false);
        audioContainer.SetActive(true);
    }

    public void VideoButton()
    {

    }

    public void ControlsButton()
    {

    }

    public void BackButton()
    {
        optionsContainer.SetActive(false);
        audioContainer.SetActive(false);
        pauseContainer.SetActive(true);
    }
}
