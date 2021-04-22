using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Deal : MonoBehaviour
{
    // Temporary Code:
    // All to test sprite replacement and deck shuffling which will be done away from deal class
    private static Sprite unflippedCard;
    public SpriteRenderer playerCard1;
    public SpriteRenderer playerCard2;
    public SpriteRenderer opponentCard1;
    public SpriteRenderer opponentCard2;
    public SpriteRenderer communityCard1;
    public SpriteRenderer communityCard2;
    public SpriteRenderer communityCard3;
    public SpriteRenderer communityCard4;
    public SpriteRenderer communityCard5;
    public TMP_Text handRankPlayer;
    public TMP_Text botRankPlayer;

    private void Awake()
    {
        unflippedCard = Resources.Load<Sprite>("Sprites/Cards/back");
    }
    public void CreateDeck()
    {
        ShowHandStrength();
        
        if (GameManager.round == 0)
        {
            GameManager.deck.RandomizeDeck();
            GameManager.round++;
            DisableAllCommunityCards();
            RevealParticipantsCards();
        }
        else if (GameManager.round == 1)
        {
            DealFlop();
            GameManager.round++;
        }
        else if (GameManager.round == 2)
        {
            DealTurn();
            GameManager.round++;
        }
        else if (GameManager.round == 3)
        {
            DealRiver();
            GameManager.round++;
        }
        else if (GameManager.round == 4)
        {
            ResetCardSpites();
            DisableAllCommunityCards();
            handRankPlayer.text = "";
            botRankPlayer.text = "";
            GameManager.round = 0;
        }
    }
    private void DealFlop()
    {
        communityCard1.sprite = GameManager.deck.deck[4].Sprite;
        communityCard2.sprite = GameManager.deck.deck[5].Sprite;
        communityCard3.sprite = GameManager.deck.deck[6].Sprite;
		communityCard1.enabled = true;
        communityCard2.enabled = true;
        communityCard3.enabled = true;
    }
    private void DealTurn()
    {
        communityCard4.sprite = GameManager.deck.deck[7].Sprite;
		communityCard4.enabled = true;
    }
    private void DealRiver()
    {
        communityCard5.sprite = GameManager.deck.deck[8].Sprite;
		communityCard5.enabled = true;
    }
    private void RevealParticipantsCards()
    {
        playerCard1.sprite = GameManager.deck.deck[0].Sprite;
        playerCard2.sprite = GameManager.deck.deck[2].Sprite;
        opponentCard1.sprite = GameManager.deck.deck[1].Sprite;
        opponentCard2.sprite = GameManager.deck.deck[3].Sprite;
    }
    private void DisableAllCommunityCards()
    {
        communityCard1.enabled = false;
        communityCard2.enabled = false;
        communityCard3.enabled = false;
        communityCard4.enabled = false;
        communityCard5.enabled = false;
    }
    private void ResetCardSpites()
    {
        playerCard1.sprite = unflippedCard;
        playerCard2.sprite = unflippedCard;
        opponentCard1.sprite = unflippedCard;
        opponentCard2.sprite = unflippedCard;
        communityCard1.sprite = unflippedCard;
        communityCard2.sprite = unflippedCard;
        communityCard3.sprite = unflippedCard;
        communityCard4.sprite = unflippedCard;
        communityCard5.sprite = unflippedCard;
    }
    private void ShowHandStrength()
    {
        List<Card> player = new List<Card>();
        List<Card> bot = new List<Card>();
        List<Card> community = new List<Card>();
        player.Add(GameManager.deck.deck[0]);
        player.Add(GameManager.deck.deck[2]);
        bot.Add(GameManager.deck.deck[1]);
        bot.Add(GameManager.deck.deck[3]);

        if(GameManager.round == 0)
        {

        }
        else if(GameManager.round == 1)
        {
            community.Add(GameManager.deck.deck[4]);
            community.Add(GameManager.deck.deck[5]);
            community.Add(GameManager.deck.deck[6]);
        }
        else if(GameManager.round == 2)
        {
            community.Add(GameManager.deck.deck[4]);
            community.Add(GameManager.deck.deck[5]);
            community.Add(GameManager.deck.deck[6]);
            community.Add(GameManager.deck.deck[7]);
        }
        else
        {
            community.Add(GameManager.deck.deck[4]);
            community.Add(GameManager.deck.deck[5]);
            community.Add(GameManager.deck.deck[6]);
            community.Add(GameManager.deck.deck[7]);
            community.Add(GameManager.deck.deck[8]);
        }

        Hand playerHand = HandStrength.DetermineHandStrength(community, player);
        Hand botHand = HandStrength.DetermineHandStrength(community, bot);
        handRankPlayer.text = playerHand.Rank.Name;
        botRankPlayer.text = botHand.Rank.Name;
    }
}