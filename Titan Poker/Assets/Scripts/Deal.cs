using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deal : MonoBehaviour
{
    // Temporary Code:
    // All to test sprite replacement and deck shuffling which will be done away from deal class

    public SpriteRenderer playerCard1;
    public SpriteRenderer playerCard2;
    public SpriteRenderer opponentCard1;
    public SpriteRenderer opponentCard2;
    public SpriteRenderer communityCard1;
    public SpriteRenderer communityCard2;
    public SpriteRenderer communityCard3;
    public SpriteRenderer communityCard4;
    public SpriteRenderer communityCard5;

    public void CreateDeck()
    {
        if (GameManager.round == 1)
        {
            communityCard1.sprite = GameManager.deck.deck[4].Sprite;
            communityCard2.sprite = GameManager.deck.deck[5].Sprite;
            communityCard3.sprite = GameManager.deck.deck[6].Sprite;
            GameManager.round++;
        }
        else if (GameManager.round == 2)
        {
            communityCard4.sprite = GameManager.deck.deck[7].Sprite;
            GameManager.round++;
        }
        else if (GameManager.round == 3)
        {
            communityCard5.sprite = GameManager.deck.deck[8].Sprite;
            GameManager.round++;
        }
        else if (GameManager.round == 4)
        {
            playerCard1.sprite = GameManager.deck.deck[0].Sprite;
            playerCard2.sprite = GameManager.deck.deck[1].Sprite;
            opponentCard1.sprite = GameManager.deck.deck[2].Sprite;
            opponentCard2.sprite = GameManager.deck.deck[3].Sprite;
            GameManager.round = 1;
        }
    }
}
