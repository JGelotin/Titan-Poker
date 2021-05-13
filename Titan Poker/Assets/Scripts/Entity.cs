using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Action
{
    NONE,
    CHECK,
    PRERAISE,
    RAISE,
    CALL,
    FOLD
}

public class Entity
{
    private Action action;
    private int betAmount;
    private int blindAmount;
    private int chipAmount;
    private List<Card> holeCards;
    private Hand hand;

    public Action Action { get { return action; } }
    public int BetAmount { get { return betAmount; } }
    public int BlindAmount { get { return blindAmount; } }
    public int ChipAmount { get { return chipAmount; } }
    public List<Card> HoleCards { get { return holeCards; } }
    public Hand Hand { get { return hand; } }

    public Entity()
    {
        action = Action.NONE;
        betAmount = 0;
        blindAmount = 0;
        chipAmount = 1000;
        holeCards = new List<Card>();
        hand = new Hand();
    }
    public void SetHoleCards(Card first, Card second)
    {
        List<Card> temp = new List<Card>();
        temp.Add(first);
        temp.Add(second);
        holeCards = new List<Card>(temp);
    }
    public void SetBlind(int blindAmount)
    {
        this.blindAmount = blindAmount;
    }
    public void SetAction(Action action)
    {
        this.action = action;
    }
    public void SetBetAmount(int betAmount)
    {
        this.betAmount = betAmount;
        
        if(betAmount < chipAmount)
            SubtractChipAmount(betAmount);
        else
        {
            this.betAmount = chipAmount;
            SubtractChipAmount(chipAmount);
        }
    }
    public void SubtractChipAmount(int chipAmount)
    {
        this.chipAmount -= chipAmount;
    }
    public void AddChipAmount(int chipAmount)
    {
        this.chipAmount += chipAmount;
    }
    public void UpdateHand(List<Card> communityCards)
    {
        hand = HandStrength.DetermineHandStrength(communityCards, holeCards);
    }
}
