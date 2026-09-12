using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;



public class PauseMenu : MonoBehaviour
{

    public GameObject pauseContainer;
    public GameObject optionsContainer;
    public GameObject audioContainer;
    public GameObject videoContainer;
    public GameObject controlsContainer;
    public GameObject keyboardContainer;
    public GameObject gamepadContainer;

    public InputActionAsset inputActions;
    


    [SerializeField] private GameObject _pauseMenuFirst;
    [SerializeField] private GameObject _settingsMenuFirst;
    [SerializeField] private GameObject _audioMenuFirst;
    [SerializeField] private GameObject _videoMenuFirst;
    [SerializeField] private GameObject _controlsMenuFirst;
    [SerializeField] private GameObject _keyboardMenuFirst;
    [SerializeField] private GameObject _gamepadMenuFirst;



    public PlayerMovement playermovement;


    public bool isPaused;

    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    private SoundMixerManager soundMixerManager;

    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;

    public TMP_Dropdown qualityDropdown;

    private List<Resolution> resolutions = new List<Resolution>();


    private void Awake()
    {
        LoadRebinds();

    }
    private void Start()
    {
        isPaused = false;


        BuildResolutionDropdown();
        qualityDropdown.onValueChanged.AddListener(SetQuality);

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
            EventSystem.current.SetSelectedGameObject(_pauseMenuFirst);
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

        int qualityIndex = PlayerPrefs.GetInt("Quality");
        SetQuality(qualityIndex);
        qualityDropdown.value = qualityIndex;
        qualityDropdown.RefreshShownValue();


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

    public void LoadRebinds()
    {
        string rebinds = PlayerPrefs.GetString("rebinds");

        if (!string.IsNullOrEmpty(rebinds))
        {
            inputActions.LoadBindingOverridesFromJson(rebinds);
        }

       
    }

  

    public void SaveRebinds()
    {
        string rebinds = inputActions.SaveBindingOverridesAsJson();

        PlayerPrefs.SetString("rebinds", rebinds);
        PlayerPrefs.Save();
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


    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("Quality", qualityIndex);
    }

    public void ResumeButton()
    {
        //Debug.Log("Resume button clicked!");
        EventSystem.current.SetSelectedGameObject(null);
        pauseContainer.SetActive(false);
        //optionsContainer.SetActive(false);
        //audioContainer.SetActive(false);
        //videoContainer.SetActive(false);
        //controlsContainer.SetActive(false);
        BackButton();
        pauseContainer.SetActive(false);
        
        Time.timeScale = 1;
        isPaused = false;
    }

    public void SettingsButton()
    {
        pauseContainer.SetActive(false);
        optionsContainer.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_settingsMenuFirst);
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
        EventSystem.current.SetSelectedGameObject(_audioMenuFirst);
        optionsContainer.SetActive(false);
        audioContainer.SetActive(true);
    }

    public void VideoButton()
    {
        EventSystem.current.SetSelectedGameObject(_videoMenuFirst);
        optionsContainer.SetActive(false);
        videoContainer.SetActive(true);
    }

    public void ControlsButton()
    {
        EventSystem.current.SetSelectedGameObject(_controlsMenuFirst);
        optionsContainer.SetActive(false);
        controlsContainer.SetActive(true);

    }

    public void KeyboardButton()
    {
        EventSystem.current.SetSelectedGameObject(_keyboardMenuFirst);
        controlsContainer.SetActive(false);
        keyboardContainer.SetActive(true);

    }
    public void GamepadButton()
    {
        EventSystem.current.SetSelectedGameObject(_gamepadMenuFirst);
        controlsContainer.SetActive(false);
        gamepadContainer.SetActive(true);

    }


    public void BackButton()
    {
        
        EventSystem.current.SetSelectedGameObject(_pauseMenuFirst);
        SaveRebinds();

        keyboardContainer.SetActive(false);
        gamepadContainer.SetActive(false);
        optionsContainer.SetActive(false);
        controlsContainer.SetActive(false);
        audioContainer.SetActive(false);
        videoContainer.SetActive(false);
        pauseContainer.SetActive(true);
    }
}
