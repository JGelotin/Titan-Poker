using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public static ActionManager instance;

    public delegate void PlayerActionMade();
    public static event PlayerActionMade playerActionMade;

    public UnityEngine.UI.Button checkButton;
    public UnityEngine.UI.Button raiseButton;
    public UnityEngine.UI.Button foldButton;
    public UnityEngine.UI.Button callButton;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
