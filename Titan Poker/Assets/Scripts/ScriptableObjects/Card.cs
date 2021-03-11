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

public class Card : MonoBehaviour
{
    CardSuit suit;
    CardType type;
    Sprite sprite;

    public CardSuit Suit { get { return suit; } }
    public CardType Type { get { return type; } }
    public Sprite Sprite { get { return sprite; } }

    public Card(CardSuit suit, CardType type)
    {
        this.suit = suit;
        this.type = type;

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
}
