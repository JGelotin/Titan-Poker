using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchingCard
{
    bool exists;
    List<Card> cards;
    int cardValue;

    public bool Exists { get { return exists; } }
    public List<Card> Cards { get { return cards; } }
    public int CardValue { get { return cardValue; } }

    public MatchingCard()
    {
        exists = false;
        cards = new List<Card>();
        cardValue = 0;
    }
    public MatchingCard(bool exists, List<Card> cards) 
    {
        this.exists = exists;
        this.cards = new List<Card>(cards);
        cardValue = cards[0].Value;
    }
}
