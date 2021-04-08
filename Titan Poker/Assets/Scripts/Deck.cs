using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    const int NUMBER_OF_CARDS = 52;
    public List<Card> deck;
    
    public Deck()
    {
        deck = new List<Card>();
        InitializeDeck();
    }
    public void InitializeDeck()
    {
        int i = 0;
        foreach(CardSuit s in Enum.GetValues(typeof(CardSuit)))
        {
            foreach (CardType t in Enum.GetValues(typeof(CardType)))
            {
                deck.Add(new Card(s, t));
                Debug.Log("Added new card");
                i++;
            }
        }
    }
    public void RandomizeDeck()
    {
        System.Random rand = new System.Random();

        for (int i = 0; i < NUMBER_OF_CARDS; i++)
        {
            int j = rand.Next(i, deck.Count);
            Card temp = deck[i];
            deck[i] = deck[j];
            deck[j] = temp;
        }
    }
}
