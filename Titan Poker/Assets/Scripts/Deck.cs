using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
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
}
