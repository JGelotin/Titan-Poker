using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flush
{
    bool exists;
    List<Card> cards;

    public bool Exists { get {return exists; } }
    public List<Card> Cards { get {return cards; } }

    public Flush()
    {
        exists = false;
        cards = new List<Card>();
    }
    public Flush(bool exists, List<Card> cards)
    {
        this.exists = exists;
        this.cards = new List<Card>(cards);
    }
}
