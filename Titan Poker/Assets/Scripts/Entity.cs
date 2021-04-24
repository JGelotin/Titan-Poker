using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Action
{
    NONE,
    CHECK,
    RAISE,
    CALL,
    FOLD
}

public class Entity
{
    private Action action;
    private int blindAmount;
    private int chipAmount;
    private List<Card> holeCards;
    private Hand hand;

    public Action Action { get { return action; } }
    public int BlindAmount { get { return blindAmount; } }
    public int ChipAmount { get { return chipAmount; } }
    public List<Card> HoleCards { get { return holeCards; } }
    public Hand Hand { get { return hand; } }

    public Entity()
    {
        action = Action.NONE;
        blindAmount = 0;
        chipAmount = 1000;
        holeCards = new List<Card>();
        hand = new Hand();
    }
    public void SetNewHoleCards(Card first, Card second)
    {
        holeCards.Clear();
        holeCards.Add(first);
        holeCards.Add(second);
    }
    public void AssignBlind(int blindAmount)
    {
        this.blindAmount = blindAmount;
    }
    public void SetAction(Action action)
    {
        this.action = action;
    }
    public void SubtractChipAmount(int chipAmount)
    {
        this.chipAmount -= chipAmount;
    }
    public void AddChipAmount(int chipAmount)
    {
        this.chipAmount += chipAmount;
    }
    public void UpdateHand(List<Card> communityCards, List<Card> holeCards)
    {
        hand = HandStrength.DetermineHandStrength(communityCards, holeCards);
    }
}
