using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EntityManager : MonoBehaviour
{
    public static EntityManager instance;

    public TMP_Text playerHandRank;
    public TMP_Text playerChipAmount;
    public TMP_Text botHandRank;
    public TMP_Text botChipAmount;

    private static Entity player;
    private static Entity bot;

    public static Entity Player { get { return player; } }
    public static Entity Bot { get { return bot; } }
    
    void Start() 
    {
        MakeSingleton();
        player = new Entity();
        bot = new Entity();
        SetEntityDefaults();

        Debug.Log("ENTITY MANAGER LOADED");
    }
    void Update() 
    {

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
    private void DisableBotHandStrength() 
    { 
        botHandRank.enabled = false; 
    }
    private void EnableBotHandStrength() 
    { 
        botHandRank.enabled = true; 
    }
    private void SetEntityDefaults()
    {
        player.AssignBlind(RoundManager.Blind.SmallAmount);
        bot.AssignBlind(RoundManager.Blind.SmallAmount);
    }
    private static void DistributeCards()
    {
        player.SetNewHoleCards(GameManager.Deck.Cards[0], GameManager.Deck.Cards[2]);
        bot.SetNewHoleCards(GameManager.Deck.Cards[0], GameManager.Deck.Cards[2]);
    }
    private void UpdateHandRanks()
    {
        playerHandRank.text = player.Hand.Rank.Name;
        botHandRank.text = bot.Hand.Rank.Name;
    }
    private void UpdateChipAmounts()
    {
        playerChipAmount.text = player.ChipAmount.ToString();
        botChipAmount.text = bot.ChipAmount.ToString();
    }
    private static void UpdateEntitiesHand()
    {
        player.UpdateHand(RoundManager.CommunityCards, player.HoleCards);
        bot.UpdateHand(RoundManager.CommunityCards, bot.HoleCards);
    }

}
