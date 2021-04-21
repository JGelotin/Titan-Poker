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

public class Card
{
    CardSuit suit;
    CardType type;
    int value;
    bool isAce;
    Sprite sprite;

    public CardSuit Suit { get { return suit; } }
    public CardType Type { get { return type; } }
    public Sprite Sprite { get { return sprite; } }
    public int Value { get { return value; } }
    public bool IsAce { get { return isAce; } }

    public Card(CardSuit suit, CardType type)
    {
        this.suit = suit;
        this.type = type;
        value = CardTypeToValue(type);

        if(type == CardType.ACE)
            isAce = true;

        string spriteFilename = CardSuitToString(suit) + "_" + CardTypeToString(type);
        sprite = Resources.Load<Sprite>("Sprites/Cards/" + spriteFilename);
    }
    public static string CardSuitToString(CardSuit suit)
    {
        switch (suit)
        {
            case CardSuit.HEART:
                return "heart";
            case CardSuit.DIAMOND:
                return "diamond";
            case CardSuit.SPADE:
                return "spade";
            case CardSuit.CLUB:
                return "club";
            default:
                return null;
        }
    }
    public static string CardTypeToString(CardType type)
    {
        switch (type)
        {
            case CardType.TWO:
                return "two";
            case CardType.THREE:
                return "three";
            case CardType.FOUR:
                return "four";
            case CardType.FIVE:
                return "five";
            case CardType.SIX:
                return "six";
            case CardType.SEVEN:
                return "seven";
            case CardType.EIGHT:
                return "eight";
            case CardType.NINE:
                return "nine";
            case CardType.TEN:
                return "ten";
            case CardType.JACK:
                return "jack";
            case CardType.QUEEN:
                return "queen";
            case CardType.KING:
                return "king";
            case CardType.ACE:
                return "ace";
            default:
                return null;
        }
    }
    private int CardTypeToValue(CardType type)
    {
        switch(type)
        {
            case CardType.TWO:
                return 2;
            case CardType.THREE:
                return 3;
            case CardType.FOUR:
                return 4;
            case CardType.FIVE:
                return 5;
            case CardType.SIX:
                return 6;
            case CardType.SEVEN:
                return 7;
            case CardType.EIGHT:
                return 8;
            case CardType.NINE:
                return 9;
            case CardType.TEN:
                return 10;
            case CardType.JACK:
                return 11;
            case CardType.QUEEN:
                return 12;
            case CardType.KING:
                return 13;
            case CardType.ACE:
                return 14;
            default:
                return 0;
        }
    }
}
