
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class HandCompare : MonoBehaviour
{
    public static int HandCompare(Hand player, Hand bot)
    {
        //0:Win
        //1:Draw
        //2:Lose
        int player_result = 0;
        public TMP_Text playerWin;

        if (player.Rank.Value > bot.Rank.Value)
        {
            player_result = 0;

            //Print Win Sprite
            playerWinText.enabled = true;
            playerWin.text = "WIN";

            //Receive All Pot
        }
        //If condition to calculate if both have the same rank 
        //Example: Three of A kind vs Three of A KIND
        else if (player.Rank.Value == bot.Rank.Value)
      
        {
            //If player has a higher card then they win
            if(player.Hand.HighestCard.Value > bot.Hand.HighestCard.Value)
            {
                player_result = 0;

                //Print Win Sprite
                playerWinText.enabled = true;
                playerWin.text = "WIN";

                //Receive All Pot
            }
            else if(player.Hand.HighestCard.Value == bot.Hand.HighestCard.Value)
            {
                player_result = 1;

                 //Print Draw Sprite
                 playerWinText.enabled = true;
                 playerWin.text = "DRAW";

                //Split Pot
            }
            else
            {
                player_result = 2;

                //Print Lose Sprite
                playerWinText.enabled = true;
                playerWin.text = "LOSE";

                //Lose Pot
            }

        }
        else
        {
            player_result = 2;

            //Print Lose Sprite
            playerWinText.enabled = true;
            playerWin.text = "LOSE";

            //Lose Pot
        }
        return player_result;
    }

    //IN PROGRESS: PLAYER CLASS 

}
