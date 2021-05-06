using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class HandCompare
{
    public static int Compare(Hand playerHand, Hand botHand)
    {
        // 0: Win
        // 1: Draw
        // 2: Lose

        int player_result = -1;
        
        if (playerHand.Rank.Value > botHand.Rank.Value)
        {
            player_result = 0;
        }
        //If condition to calculate if both have the same rank 
        //Example: Three of A kind vs Three of A KIND
        else if (playerHand.Rank.Value == botHand.Rank.Value)
        {
            //If player has a higher card then they win
            if(playerHand.HighestCard.Value > botHand.HighestCard.Value)
                player_result = 0;

            else if(playerHand.HighestCard.Value == botHand.HighestCard.Value)
                player_result = 1;

            else
                player_result = 2;
        }
        else
            player_result = 2;

        return player_result;
    }
}
