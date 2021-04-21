using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand
{
    // Holds the information for the rank of the hand (rank type and the value of the rank)
    HandRank rank;

    // Involves all cards that can be used to give a hand a rank (Community Cards + Player Hole Cards)
    List<Card> cardsInPlay;

    // All the cards that make up the highest rank hand from the cards available to the player
    // Ex. Rank: Straight
    //     CardsInvolvedInHand: 4 5 6 7 8
    List<Card> cardsInvolvedInHand;

    Card highestCard;

    /**
     * The reason for keeping the information of multiple hands (even if that particular hand is not the strongest)
     * is because there are certain hands that require both types of hand ranks to be fulfilled.
     * For example, a Royal Flush and a Straight Flush are both a flush and a straight. So, it is important
     * to keep all cards and information regarding flushes and straights on the table to check for stronger hands
     * like the Royal Flush and Straight Flush.
     **/

    // Highest Matching Card Hand (Full House, Pair, Two Pair, etc.) from cards in play
    MatchingCard matchingCard = new MatchingCard();

    // Highest Flush Hand from cards in play
    Flush flush = new Flush();

    // Highest Straight Hand from cards in play
    Straight straight = new Straight();
    
    /**********************************************/
    /*            GETTER FUNCTIONS                */
    /**********************************************/
    public HandRank Rank { get { return rank; } }
    public List<Card> CardsInPlay { get { return cardsInPlay; } }
    public List<Card> CardsInvolvedInHand { get { return cardsInvolvedInHand; } }
    public Card HighestCard { get { return highestCard; } }
    public MatchingCard MatchingCard { get { return matchingCard; } }
    public Flush Flush { get { return flush; } }
    public Straight Straight { get { return straight; } }

    /**********************************************/
    /*              CONSTRUCTORS                  */
    /**********************************************/
    public Hand() 
    {
        this.rank = new HandRank(global::Rank.HIGHCARD);
        cardsInPlay = new List<Card>();
        cardsInvolvedInHand = new List<Card>();
        highestCard = new Card(CardSuit.HEART, CardType.TWO);
    }
    public Hand(HandRank rank, List<Card> cardsInPlay) 
    {
        this.rank = rank;
        this.cardsInPlay = new List<Card>(cardsInPlay);
        highestCard = new Card(CardSuit.HEART, CardType.TWO);
        cardsInvolvedInHand = new List<Card>();
    }
    public Hand(HandRank rank, List<Card> cardsInPlay, List<Card> cardsInvolvedInHand)
    {
        this.rank = rank;
        this.cardsInPlay = new List<Card>(cardsInPlay);
        this.cardsInvolvedInHand = new List<Card>(cardsInvolvedInHand);
        highestCard = cardsInvolvedInHand[cardsInvolvedInHand.Count - 1];
    }

    /**********************************************/
    /*            BUILDER FUNCTIONS               */
    /**********************************************/
    public void AddMatchingCard(MatchingCard matchingCard) 
    {
        this.matchingCard = matchingCard; 
    }
    public void AddFlush(Flush flush) 
    { 
        this.flush = flush;

        if(rank.Value < 6) // Current hand rank is lower than flush
        {
            rank = new HandRank(global::Rank.FLUSH);
            cardsInvolvedInHand = flush.Cards;
            highestCard = flush.Cards[flush.Cards.Count - 1];
        }
    }
    public void AddStraight(Straight straight) 
    { 
        this.straight = straight;

        if(rank.Value < 5) // Current hand rank is lower than straight
        {
            rank = new HandRank(global::Rank.STRAIGHT);
            cardsInvolvedInHand = straight.Cards;
            highestCard = straight.Cards[straight.Cards.Count - 1];
        }
    }
}
