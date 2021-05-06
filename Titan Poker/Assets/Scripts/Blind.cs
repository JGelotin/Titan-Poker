using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blind
{
    private const int MIN_BLIND_AMOUNT = 10;
    private const int MAX_BLIND_AMOUNT = 20;

    private int smallBlindAmount;
    private int bigBlindAmount;

    public int SmallAmount { get { return smallBlindAmount; } } 
    public int BigAmount { get { return bigBlindAmount; } }

    public Blind()
    {
        smallBlindAmount = MIN_BLIND_AMOUNT;
        bigBlindAmount = MAX_BLIND_AMOUNT;
    }
    public void IncreaseBlind(int multiplier)
    {
        smallBlindAmount *= multiplier;
        bigBlindAmount *= multiplier;
    }
}
