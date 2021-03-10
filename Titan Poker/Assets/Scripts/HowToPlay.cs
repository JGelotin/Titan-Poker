using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToPlay : MonoBehaviour
{
    public void AdvanceToNextHowToPlayScene()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("HandRankings_5"))
            SceneManager.LoadScene("HowToPlayMenu");
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
