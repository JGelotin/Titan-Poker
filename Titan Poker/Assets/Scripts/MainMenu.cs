using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void CreateTable()
    {
        SceneManager.LoadScene("TournamentTable");
    }
    public void HowToPlay()
    {
        SceneManager.LoadScene("HowToPlayMenu");
    }
    public void HandRankings()
    {
        SceneManager.LoadScene("HandRankings_1");
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
