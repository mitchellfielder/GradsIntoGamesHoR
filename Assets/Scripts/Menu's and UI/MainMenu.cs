using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("Hotel",LoadSceneMode.Single);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
