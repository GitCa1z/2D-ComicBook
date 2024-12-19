using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void StartGame()
    {
        // Replace "GameScene" with the name of your game scene
        SceneManager.LoadScene(0);
    }

    public void OpenOptions()
    {
        Debug.Log("Options button clicked!"); // Replace this with your options logic
    }

    public void ExitGame()
    {
        Debug.Log("Exit button clicked!");
        Application.Quit();
    }
}
