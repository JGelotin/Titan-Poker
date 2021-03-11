using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CardSuit
{
    HEART, DIAMOND, SPADE, CLUB
}
public enum CardType
{
    TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN, JACK, QUEEN, KING, ACE
}

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public CardSuit suit;
    public CardType type;
    public Sprite artwork;
}
