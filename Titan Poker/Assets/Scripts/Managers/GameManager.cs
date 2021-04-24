using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public delegate void ResetGameDelegate();
    public delegate void PlayerActionCompletedDelegate();
    public delegate void ShowdownDelegate();

    public static event ResetGameDelegate resetGameDelegate;
    public static event PlayerActionCompletedDelegate playerActionCompletedDelegate;
    public static event ShowdownDelegate showdownDelegate;

    public UnityEngine.UI.Button checkButton;
    public UnityEngine.UI.Button raiseButton;
    public UnityEngine.UI.Button foldButton;
    public UnityEngine.UI.Button callButton;

    private static Deck deck;
    public static Deck Deck { get { return deck; } }

    private void Start()
    {
        MakeSingleton();
        Debug.Log("GAME MANAGER CALLED");
        deck = new Deck();
    }
    private void MakeSingleton()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void ActionGenerated()
    {
        playerActionCompletedDelegate.Invoke();
        
    }
}
