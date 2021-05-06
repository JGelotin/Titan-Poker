// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;

// public class EntityManager : MonoBehaviour
// {
//     public static EntityManager instance;

//     public UnityEngine.UI.Button checkButton;
//     public UnityEngine.UI.Button raiseButton;
//     public UnityEngine.UI.Button callButton;
//     public UnityEngine.UI.Button foldButton;
//     public TMP_Text raiseAmountText;

//     public TMP_Text playerHandRank;
//     public TMP_Text playerChipAmount;
//     public TMP_Text botHandRank;
//     public TMP_Text botChipAmount;

//     private static Entity player;
//     private static Entity bot;

//     public static Entity Player { get { return player; } }
//     public static Entity Bot { get { return bot; } }

//     private bool playerTurn;
//     private bool entitiesReady;

//     public static bool AreEntitiesReady { get; set;}
    
//     void Start() 
//     {
//         MakeSingleton();
//         player = new Entity();
//         bot = new Entity();
//         playerTurn = false;
//         entitiesReady = false;
//         DefaultObjectLoad();

//         Debug.Log("ENTITY MANAGER LOADED");
//     }
//     void Update() 
//     {
//         if(player.Action != Action.NONE && bot.Action != Action.NONE)
//         {
//             if(player.Action == bot.Action || 
//               (player.Action == Action.CALL && bot.Action == Action.RAISE) ||
//               (player.Action == Action.RAISE && bot.Action == Action.CALL))
//               {
//                   entitiesReady = true;
//               }
//         }
//     }
//     private void MakeSingleton()
//     {
//         if(instance != null)
//         {
//             Destroy(gameObject);
//         }
//         else
//         {
//             instance = this;
//             DontDestroyOnLoad(gameObject);
//         }
//     }
//     public void UpdateUI()
//     {
//         playerHandRank.text = player.Hand.Rank.Name;
//         playerChipAmount.text = player.ChipAmount.ToString();
//         botHandRank.text = bot.Hand.Rank.Name;
//         botChipAmount.text = bot.ChipAmount.ToString();
//     }
//     /******************************************/
//     /*           BUTTON CLICK EVENTS          */
//     /******************************************/
//     public void CheckButtonClick()
//     {
//         if(bot.Action == Action.NONE || bot.Action == Action.CHECK)
//             player.SetAction(Action.CHECK);
//     }
//     public void RaiseButtonClick()
//     {
//         if(player.Action == Action.NONE)
//         {
//             player.SetAction(Action.PRERAISE);
//             checkButton.gameObject.SetActive(false);
//             callButton.gameObject.SetActive(false);
//             foldButton.gameObject.SetActive(false);
//             raiseAmountText.gameObject.SetActive(true);
//             raiseButton.transform.localPosition = new Vector3((float)-419.9, (float)-385.6, 0);
//         }
//         else
//         {
//             player.SetAction(Action.RAISE);
//             HideAllObjects();
//             player.SubtractChipAmount(Int32.Parse(raiseAmountText.text));
//             raiseAmountText.text = "";
//         }
//     }
//     public void CallButtonClick()
//     {
//         if(bot.Action == Action.RAISE || bot.BetAmount > player.BetAmount)
//         {
//             player.SetAction(Action.CALL);
//             player.SubtractChipAmount(bot.BetAmount - player.BetAmount);
//         }
//     }
//     public void FoldButtonClick()
//     {
//         player.SetAction(Action.FOLD);
//     }

//     /******************************************/
//     /*         UI OBJECT MODIFICATION         */
//     /******************************************/
//     private void DefaultObjectLoad()
//     {
//         checkButton.gameObject.SetActive(false);
//         raiseButton.gameObject.SetActive(false);
//         callButton.gameObject.SetActive(false);
//         foldButton.gameObject.SetActive(false);
//         raiseAmountText.text = "";
//         raiseAmountText.gameObject.SetActive(false);
//     }
//     private void ShowObjectsGivenOpponentNoRaise()
//     {
//         checkButton.gameObject.SetActive(true);
//         raiseButton.gameObject.SetActive(true);
//         callButton.enabled = false;
//         foldButton.gameObject.SetActive(true);
//     }
//     private void ShowObjectsGivenOpponentRaise()
//     {
//         checkButton.enabled = false;
//         raiseButton.gameObject.SetActive(true);
//         callButton.enabled = true;
//         foldButton.gameObject.SetActive(true);
//     }
//     private void HideAllObjects()
//     {
//         checkButton.gameObject.SetActive(false);
//         raiseButton.gameObject.SetActive(false);
//         callButton.gameObject.SetActive(false);
//         foldButton.gameObject.SetActive(false);
//         raiseAmountText.text = "";
//         raiseAmountText.gameObject.SetActive(false);
//     }
// }
