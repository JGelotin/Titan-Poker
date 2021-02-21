using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void CreateTable()
    {
        SceneManager.LoadScene("SampleScene2");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
