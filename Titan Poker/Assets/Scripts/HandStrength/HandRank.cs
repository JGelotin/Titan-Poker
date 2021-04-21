using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rank 
{
    HIGHCARD,
    PAIR,
    TWOPAIR,
    THREEOFAKIND,
    STRAIGHT,
    FLUSH,
    FULLHOUSE,
    FOUROFAKIND,
    STRAIGHTFLUSH,
    ROYALFLUSH
}
public class HandRank
{
    Rank type;
    int value;
    string name;

    public HandRank(Rank type)
    {
        this.type = type;
        value = RankToValue(type);
        name = RankToString(type);
    }

    public Rank Type { get { return type;} }
    public int Value { get { return value;} }
    public string Name { get { return name; } }
    
    private static int RankToValue(Rank type)
    {
        // The values returned from this function will be used to compare ranks
        switch(type)
        {
            case Rank.HIGHCARD:
                return 1;
            case Rank.PAIR:
                return 2;
            case Rank.TWOPAIR:
                return 3;
            case Rank.THREEOFAKIND:
                return 4;
            case Rank.STRAIGHT:
                return 5;
            case Rank.FLUSH:
                return 6;
            case Rank.FULLHOUSE:
                return 7;
            case Rank.FOUROFAKIND:
                return 8;
            case Rank.STRAIGHTFLUSH:
                return 9;
            case Rank.ROYALFLUSH:
                return 10;
            default:
                return 0;
        }
    }
    private static string RankToString(Rank type)
    {
        // The values returned from this function will be used to compare ranks
        switch(type)
        {
            case Rank.HIGHCARD:
                return "HIGH CARD";
            case Rank.PAIR:
                return "PAIR";
            case Rank.TWOPAIR:
                return "TWO PAIR";
            case Rank.THREEOFAKIND:
                return "THREE OF A KIND";
            case Rank.STRAIGHT:
                return "STRAIGHT";
            case Rank.FLUSH:
                return "FLUSH";
            case Rank.FULLHOUSE:
                return "FULL HOUSE";
            case Rank.FOUROFAKIND:
                return "FOUR OF A KIND";
            case Rank.STRAIGHTFLUSH:
                return "STRAIGHT FLUSH";
            case Rank.ROYALFLUSH:
                return "ROYAL FLUSH";
            default:
                return "NULL";
        }
    }
}
