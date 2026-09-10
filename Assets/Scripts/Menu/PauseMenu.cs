using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class PauseMenu : MonoBehaviour
{

    public GameObject pauseContainer;
    public GameObject optionsContainer;
    public GameObject audioContainer;
    public GameObject videoContainer;


    public PlayerMovement playermovement;

    public bool isPaused;

    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    private SoundMixerManager soundMixerManager;

    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;

    private List<Resolution> resolutions = new List<Resolution>();

    private void Start()
    {
        isPaused = false;

        BuildResolutionDropdown();

        soundMixerManager = FindObjectOfType<SoundMixerManager>();

        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        fullscreenToggle.onValueChanged.AddListener(delegate { FullScreenToggle(); });

        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);

        LoadSettings();
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
    

    void LoadSettings()
    {
        int resolutionIndex = PlayerPrefs.GetInt("resolution");
        SetResolution(resolutionIndex);

        bool fullscreen = PlayerPrefs.GetInt("fullscreen", 1) == 1;

        fullscreenToggle.isOn = fullscreen;
        Screen.fullScreen = fullscreen;

        float masterVolume = PlayerPrefs.GetFloat("masterVolume", 1f);
        float musicVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 1f);

        masterVolumeSlider.value = masterVolume;
        musicVolumeSlider.value = musicVolume;
        sfxVolumeSlider.value = sfxVolume;

        soundMixerManager.SetMasterVolume(masterVolume);
        soundMixerManager.SetMusicVolume(musicVolume);
        soundMixerManager.SetSFXVolume(sfxVolume);
    }

    public void SetMasterVolume(float value)
    {
        soundMixerManager.SetMasterVolume(value);
        PlayerPrefs.SetFloat("masterVolume", value);
    }

    public void SetMusicVolume(float value)
    {
        soundMixerManager.SetMusicVolume(value);
        PlayerPrefs.SetFloat("musicVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        soundMixerManager.SetSFXVolume(value);
        PlayerPrefs.SetFloat("sfxVolume", value);
    }

    public void FullScreenToggle()
    {
       
          Screen.fullScreen = fullscreenToggle.isOn;
          PlayerPrefs.SetInt("fullscreen", fullscreenToggle.isOn ? 1 : 0);
   
    }
    void BuildResolutionDropdown()
    {
        resolutions.Clear();
        List<string> options = new List<string>();
        
        foreach(Resolution res in Screen.resolutions)
        {
            if (resolutions.Exists(r => r.width == res.width && r.height == res.height)) continue;

            resolutions.Add(res);
            options.Add(res.width + " x " + res.height);
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);
    }

    public void SetResolution(int index)
    {
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        resolutionDropdown.value = index;

        PlayerPrefs.SetInt("resolution", index);
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
        optionsContainer.SetActive(false);
        videoContainer.SetActive(true);
    }

    public void ControlsButton()
    {

    }

    public void BackButton()
    {
        PlayerPrefs.Save();

        optionsContainer.SetActive(false);
        audioContainer.SetActive(false);
        videoContainer.SetActive(false);
        pauseContainer.SetActive(true);
    }
}
