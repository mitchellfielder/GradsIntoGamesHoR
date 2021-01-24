using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private AudioMixerSnapshot _paused;
    [SerializeField]
    private AudioMixerSnapshot _unpaused;

    void OnEnable()
    {
        _paused.TransitionTo(0.0f);
    }

    public void Continue()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1.0f;
        _unpaused.TransitionTo(0.0f);

    }

    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
        
    }


    public void Quit()
    {
        Application.Quit();
    }

}
