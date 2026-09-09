using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{

    public GameObject container;
    public PlayerMovement playermovement;
    public bool isPaused;

    private void Start()
    {
        isPaused = false;
    }
    void Update()
    {
        if (playermovement.menuAction && !isPaused && Time.timeScale != 0)
        {
           // Cursor.lockState = CursorLockMode.None;
            container.SetActive(true);
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
        container.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    public void SettingsButton()
    {
        
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
}
