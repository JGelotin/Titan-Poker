using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    public Card card;
    public SpriteRenderer spriteRenderer;
    public Sprite cardArt;

    void Start()
    {
        spriteRenderer.sprite = card.artwork;
    }
}
