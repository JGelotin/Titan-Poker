using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HandStrength
{
    public static Hand DetermineHandStrength(List<Card> community, List<Card> playerHole) 
    {
        List<Card> availableCards = new List<Card>(community);
        availableCards.Add(playerHole[0]);
        availableCards.Add(playerHole[1]);

        availableCards = SortCards(availableCards);
        List<Card> highCard = new List<Card>();
        highCard.Add(availableCards[availableCards.Count - 1]);

        Hand hand = new Hand(new HandRank(Rank.HIGHCARD), availableCards, highCard);

        hand = CheckForSameCardRank(hand);
        hand = CheckForFlush(hand);
        hand = CheckForStraight(hand);

        if(hand.Straight.Exists && hand.Flush.Exists)
        {
            hand = CheckStraightFlush(hand);
            return hand;
        }
        
        return hand;
    }
    private static List<Card> SortCards(List<Card> cards)
    {
        if(cards.Count <= 1)
            return cards;
        else
        {
            int pivot = cards[0].Value;
            List<Card> lt = new List<Card>();
            List<Card> eq = new List<Card>();
            List<Card> gt = new List<Card>();

            for(int i = 0; i < cards.Count; i++)
            {
                if(cards[i].Value < pivot)
                    lt.Add(cards[i]);
                else if(cards[i].Value == pivot)
                    eq.Add(cards[i]);
                else
                    gt.Add(cards[i]);
            }

            List<Card> sorted_lt = SortCards(lt);
            List<Card> sorted_eq = new List<Card>(eq);
            List<Card> sorted_gt = SortCards(gt);
            List<Card> sorted = new List<Card>();

            for(int i = 0; i < sorted_lt.Count; i++)
                sorted.Add(sorted_lt[i]);
            for(int i = 0; i < sorted_eq.Count; i++)
                sorted.Add(sorted_eq[i]);
            for(int i = 0; i < sorted_gt.Count; i++)
                sorted.Add(sorted_gt[i]);
            
            return sorted;
        }
    }
    private static Hand CheckForSameCardRank(Hand hand)
    {
        List<MatchingCard> allMatchingHands = FindAllSameCardRankHands(hand);
        List<Card> allCardsInPlay = new List<Card>(hand.CardsInPlay);

        /**********************************************/
        /*          0 FOUND MATCHING HANDS            */
        /**********************************************/

        if (allMatchingHands.Count == 0)
        {
            return hand;
        }

        /**********************************************/
        /*          1 FOUND MATCHING HANDS            */
        /**********************************************/
        
        else if (allMatchingHands.Count == 1)
        {
            /**** PAIR ****/
            if(allMatchingHands[0].Cards.Count == 2)
                return new Hand(new HandRank(Rank.PAIR), allCardsInPlay, allMatchingHands[0].Cards);
            
            /**** THREE OF A KIND ****/
            else if (allMatchingHands[0].Cards.Count == 3)
                return new Hand(new HandRank(Rank.THREEOFAKIND), allCardsInPlay, allMatchingHands[0].Cards);
            
            /**** FOUR OF A KIND ****/
            else
                return new Hand(new HandRank(Rank.FOUROFAKIND), allCardsInPlay, allMatchingHands[0].Cards);
        }

        /**********************************************/
        /*          2 FOUND MATCHING HANDS            */
        /**********************************************/

        else if (allMatchingHands.Count == 2)
        {
            // Created separate class instances for readability
            MatchingCard firstHand = allMatchingHands[0];
            MatchingCard secondHand = allMatchingHands[1];

            /***************************/
            /*  FIRST HAND IS A PAIR   */
            /***************************/
            if (firstHand.Cards.Count == 2)
            {
                /**** TWO PAIR ****/
                if (secondHand.Cards.Count == 2)
                {
                    List<Card> cardsInHand = CombineCardsOfHands(firstHand, secondHand);
                    return new Hand(new HandRank(Rank.TWOPAIR), allCardsInPlay, cardsInHand);
                }
                /**** FULL HOUSE ****/
                else if (secondHand.Cards.Count == 3)
                {
                    List<Card> cardsInHand = CombineCardsOfHands(firstHand, secondHand);
                    return new Hand(new HandRank(Rank.FULLHOUSE), allCardsInPlay, cardsInHand);
                }
                /**** FOUR OF A KIND ****/
                else
                {
                    // Since Four of a Kind > Pair, Four of a Kind takes precedence
                    return new Hand(new HandRank(Rank.FOUROFAKIND), allCardsInPlay, secondHand.Cards);
                }
            }

            /**************************************/
            /*  FIRST HAND IS A THREE OF A KIND   */
            /**************************************/

            else if (firstHand.Cards.Count == 3)
            {
                /**** FULL HOUSE ****/
                if (secondHand.Cards.Count == 2)
                {
                    List<Card> cardsInHand = CombineCardsOfHands(firstHand, secondHand);
                    return new Hand(new HandRank(Rank.FULLHOUSE), allCardsInPlay, cardsInHand);
                }
                /**** THREE OF A KIND (CHOOSE HIGHER CARD VALUE) ****/
                else if (secondHand.Cards.Count == 3)
                {
                    if(firstHand.CardValue > secondHand.CardValue)
                        return new Hand(new HandRank(Rank.THREEOFAKIND), allCardsInPlay, firstHand.Cards);

                    else
                        return new Hand(new HandRank(Rank.THREEOFAKIND), allCardsInPlay, secondHand.Cards);
                }
                else
                {
                    // Since Four of a Kind > Three of a Kind, Four of a Kind takes precedence
                    return new Hand(new HandRank(Rank.FOUROFAKIND), allCardsInPlay, secondHand.Cards);
                }
            }

            /*************************************/
            /*  FIRST HAND IS A FOUR OF A KIND   */
            /*************************************/

            else
            {
                return new Hand(new HandRank(Rank.FOUROFAKIND), allCardsInPlay, firstHand.Cards);
            }
        }

        /**********************************************/
        /*          3 FOUND MATCHING HANDS            */
        /**********************************************/

        else
        {
            /**
             * The largest possible hand to get if there are three matching card hands is a three of a kind
             * (7 Cards Maximum In Play -> Three of a Kind + Pair + Pair)
             *
             * So the best approach is to check if all three hands are pairs and if one hand is a three of a kind
             * then compare the card values of each hand.
             **/


            // Created separate class instances for readability
            MatchingCard firstHand = allMatchingHands[0];
            MatchingCard secondHand = allMatchingHands[1];
            MatchingCard thirdHand = allMatchingHands[2];

            /*************************************/
            /*      ALL 3 HANDS ARE PAIRS        */
            /*************************************/
            if (firstHand.Cards.Count == 2 && secondHand.Cards.Count == 2 && thirdHand.Cards.Count == 2)
            {
                /**** FIRST PAIR HIGHEST ****/
                if (firstHand.CardValue > secondHand.CardValue && firstHand.CardValue > thirdHand.CardValue)
                    return new Hand(new HandRank(Rank.PAIR), allCardsInPlay, firstHand.Cards);
                
                /**** SECOND PAIR STRONGEST ****/
                else if (secondHand.CardValue > firstHand.CardValue && secondHand.CardValue > thirdHand.CardValue)
                    return new Hand(new HandRank(Rank.PAIR), allCardsInPlay, secondHand.Cards);

                /**** THIRD PAIR STRONGEST ****/
                else
                    return new Hand(new HandRank(Rank.PAIR), allCardsInPlay, thirdHand.Cards);
            }

            /*************************************/
            /*      THREE OF A KIND CHECK        */
            /*************************************/

            else if (firstHand.Cards.Count == 3)
                return new Hand(new HandRank(Rank.THREEOFAKIND), allCardsInPlay, firstHand.Cards); 

            else if (secondHand.Cards.Count == 3)
                return new Hand(new HandRank(Rank.THREEOFAKIND), allCardsInPlay, secondHand.Cards);

            else
                return new Hand(new HandRank(Rank.THREEOFAKIND), allCardsInPlay, thirdHand.Cards);
        }
    }
    private static List<Card> CombineCardsOfHands(MatchingCard hand1, MatchingCard hand2)
    {
        List<Card> cardsInHand = new List<Card>(hand1.Cards);

        for (int i = 0; i < hand2.Cards.Count; i++)
            cardsInHand.Add(hand2.Cards[i]);

        return cardsInHand;
    }
    private static List<MatchingCard> FindAllSameCardRankHands(Hand hand)
    {
        // Hold all pairs/three-of-a-kinds/four-of-a-kind
        List<MatchingCard> allFoundHands = new List<MatchingCard>();

        // Keeps track of all cards that are a part of a pair/three-of-a-kind/four-of-a-kind
        // This list of cards gets added into a list of same-card hands (allFoundHands) for later
        List<Card> foundMatchingCards = new List<Card>();

        for (int i = 0; i < hand.CardsInPlay.Count; i++)
        {
            if(i == 0)
            {
                foundMatchingCards.Add(hand.CardsInPlay[0]);
            }
            // Matching denomination has been found
            else if(hand.CardsInPlay[i].Value == hand.CardsInPlay[i - 1].Value)
            {
                foundMatchingCards.Add(hand.CardsInPlay[i]);

                if(i == hand.CardsInPlay.Count - 1) // Last card to check
                {
                    MatchingCard found = new MatchingCard(true, foundMatchingCards);
                    allFoundHands.Add(found);
                    foundMatchingCards.Clear();
                }
            }
            else
            {
                // [FOUND] At least a pair is found of same card rank
                if(foundMatchingCards.Count > 1)
                {
                    MatchingCard found = new MatchingCard(true, foundMatchingCards);
                    allFoundHands.Add(found);
                    foundMatchingCards.Clear();
                    foundMatchingCards.Add(hand.CardsInPlay[i]);
                }
                else
                {
                    foundMatchingCards.Clear();
                    foundMatchingCards.Add(hand.CardsInPlay[i]);
                }
            }
        }
        foundMatchingCards.Clear();
        return allFoundHands;
    }
    private static Hand CheckForFlush(Hand hand)
    {
        /** 
         * The reason for not creating containers for every found suit is because
         * a flush is not a common occurence. The average case of each round is a non-flush hand
         * so it wastes time and resources creating new collections for every hand check
         **/ 

        int numOfHearts = 0;
        int numOfDiamonds = 0;
        int numOfClubs = 0;
        int numOfSpades = 0;

        for (int i = 0; i < hand.CardsInPlay.Count; i++) 
        {
            if (hand.CardsInPlay[i].Suit == CardSuit.HEART)
                numOfHearts++;
            else if (hand.CardsInPlay[i].Suit == CardSuit.DIAMOND)
                numOfDiamonds++;
            else if (hand.CardsInPlay[i].Suit == CardSuit.CLUB)
                numOfClubs++;
            else
                numOfSpades++;
        }

        if(numOfHearts >= 5 || numOfDiamonds >= 5 || numOfClubs >= 5 || numOfSpades >= 5)
        {
            List<Card> flushCards = new List<Card>();

            if(numOfHearts >= 5)
                flushCards = GetAllCardsOfSameSuit(hand.CardsInPlay, CardSuit.HEART);
            
            else if (numOfDiamonds >= 5) 
                flushCards = GetAllCardsOfSameSuit(hand.CardsInPlay, CardSuit.DIAMOND);

            else if (numOfClubs >= 5) 
                flushCards = GetAllCardsOfSameSuit(hand.CardsInPlay, CardSuit.CLUB);

            else
                flushCards = GetAllCardsOfSameSuit(hand.CardsInPlay, CardSuit.SPADE);

            Flush flush = new Flush(true, flushCards);
            Hand temp = hand;
            temp.AddFlush(flush);

            numOfHearts = 0;
            numOfDiamonds = 0;
            numOfClubs = 0;
            numOfSpades = 0;

            return temp;
        }
        else
        {
            numOfHearts = 0;
            numOfDiamonds = 0;
            numOfClubs = 0;
            numOfSpades = 0;

            return hand;
        }
    }
    private static List<Card> GetAllCardsOfSameSuit(List<Card> cards, CardSuit targetSuit)
    {
        List<Card> suitedCards = new List<Card>();
        for(int i = 0; i < cards.Count; i++)
        {
            if(cards[i].Suit == targetSuit)
                suitedCards.Add(cards[i]);
        }

        /** 
         * The reason for returning "all" cards with the same suit and not just 5 cards is because
         * the player might have 7 cards with the same suit but also a straight at the same time.
         * If you remove certain cards, the cards leading to the straight might not be there, therefore
         * incorrectly giving the real strength of the hand
         **/ 
        return suitedCards;
    }
    private static Hand CheckForStraight(Hand hand)
    {
        List<Card> cardsInStraight = new List<Card>();

        /***** CHECK IF ENOUGH CARDS ARE IN NON-DECREASING ORDER *****/
        int cardsInNonDecreasingOrder = 1;
        for(int i = 1; i < hand.CardsInPlay.Count; i++)
        {
            if(hand.CardsInPlay[i].Value == hand.CardsInPlay[i-1].Value + 1 || hand.CardsInPlay[i].Value == hand.CardsInPlay[i-1].Value)
                cardsInNonDecreasingOrder++;

            else
            {
                if(cardsInNonDecreasingOrder < 5)
                    cardsInNonDecreasingOrder = 1;
            }
        }

        if(cardsInNonDecreasingOrder < 5)
            return hand;

        /***** CHECK IF CARDS IN NON-DECREASING ORDER FORM A STRAIGHT *****/
        int uniqueNonDecreasingOrderCards = 1;
        for(int i = 1; i < hand.CardsInPlay.Count; i++)
        {
            if(hand.CardsInPlay[i].Value == hand.CardsInPlay[i-1].Value + 1)
            {
                uniqueNonDecreasingOrderCards++;
            }
            else if(hand.CardsInPlay[i].Value == hand.CardsInPlay[i-1].Value)
            {
                continue;
            }
            else
            {
                if(uniqueNonDecreasingOrderCards < 5)
                    uniqueNonDecreasingOrderCards = 1;
            }
        }

        if(uniqueNonDecreasingOrderCards < 5)
            return hand;

        /***** GET CARDS THAT FORM THE STRAIGHT *****/
        List<Card> cardsThatFormStraight = new List<Card>();
        for(int i = 0; i < hand.CardsInPlay.Count; i++)
        {
            if(i == 0) 
                cardsThatFormStraight.Add(hand.CardsInPlay[i]); 

            else if(hand.CardsInPlay[i].Value == hand.CardsInPlay[i-1].Value + 1 ||
                    hand.CardsInPlay[i].Value == hand.CardsInPlay[i-1].Value)
            {
                    cardsThatFormStraight.Add(hand.CardsInPlay[i]); 

                    if(i == hand.CardsInPlay.Count - 1)
                    {
                        Straight straight = new Straight(true, cardsThatFormStraight);
                        Hand temp = hand;
                        temp.AddStraight(straight);
                        return temp;
                    }
            }
   
            else
            {
                if(cardsThatFormStraight.Count >= 5)
                {
                    Straight straight = new Straight(true, cardsThatFormStraight);
                    Hand temp = hand;
                    temp.AddStraight(straight);
                    return temp;
                }
                else
                {
                    cardsThatFormStraight.Clear();
                    cardsThatFormStraight.Add(hand.CardsInPlay[i]);
                }
            }
        }

        return hand;
    }
    private static Hand CheckStraightFlush(Hand hand)
    {
        Flush flush = hand.Flush;
        Straight straight = hand.Straight;

        Hashtable cards = new Hashtable();

        for(int i = 0; i < flush.Cards.Count; i++)
        {
            cards.Add(flush.Cards[i].Value, flush.Cards[i]);
        }

        List<Card> straightFlush = new List<Card>();

        for(int i = 0; i < straight.Cards.Count; i++)
        {
            if(cards.Contains(straight.Cards[i].Value))
                straightFlush.Add(straight.Cards[i]);
        }

        if(straightFlush.Count >= 5)
        {
            if(straightFlush[straightFlush.Count - 1].Type == CardType.ACE)
            {
                Hand temp = new Hand(new HandRank(Rank.ROYALFLUSH), hand.CardsInPlay, straightFlush);
                return temp;
            }
            else
            {
                Hand temp = new Hand(new HandRank(Rank.STRAIGHTFLUSH), hand.CardsInPlay, straightFlush);
                return temp;
            }
        }
        else
        {
            return hand;
        }
    }
}