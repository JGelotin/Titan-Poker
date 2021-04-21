using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Straight
{
    bool exists;
    List<Card> cards;

    public bool Exists { get {return exists; } }
    public List<Card> Cards { get {return cards; } }
    
    public Straight()
    {
        exists = false;
        cards = new List<Card>();
    }
    public Straight(bool exists, List<Card> cards)
    {
        this.exists = exists;
        this.cards = new List<Card>(cards);
    }
}
