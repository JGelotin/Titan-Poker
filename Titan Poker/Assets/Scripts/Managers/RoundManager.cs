using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Round
{
    PREFLOP,
    FLOP,
    TURN,
    RIVER,
    SHOWDOWN
}

public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;

    public delegate void ResetRoundDelegate();
    public delegate void PlayerActionDelegate();
    public delegate void ShowdownDelegate();

    public static event ResetRoundDelegate resetGameDelegate;
    public static event PlayerActionDelegate playerActionCompletedDelegate;
    public static event ShowdownDelegate showdownDelegate;

    private static Sprite unflippedCard;
    public SpriteRenderer playerCard1;
    public SpriteRenderer playerCard2;
    public SpriteRenderer opponentCard1;
    public SpriteRenderer opponentCard2;
    public SpriteRenderer communityCard1;
    public SpriteRenderer communityCard2;
    public SpriteRenderer communityCard3;
    public SpriteRenderer communityCard4;
    public SpriteRenderer communityCard5;

    private static List<Card> communityCards;
    private static Round round;
    private static Blind blind;
    private static int gamesPassed;

    public static List<Card> CommunityCards { get { return communityCards; } }
    public static Round Round { get { return round; } }
    public static Blind Blind { get { return blind; } }

    /*************************************************/
    /*                UNITY FUNCTIONS                */
    /*************************************************/
    void Start()
    {
        MakeSingleton();
        unflippedCard = Resources.Load<Sprite>("Sprites/Cards/back");
        communityCards = new List<Card>();
        round = Round.PREFLOP;
        blind = new Blind();
        gamesPassed = 0;

        Debug.Log("ROUND MANAGER CALLED");
    }
    private void MakeSingleton()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    void Update() {}

    /*************************************************/
    /*                 SET DELEGATES                 */
    /*************************************************/
    private void UpdateRound()
    {

    }

    /*************************************************/
    /*          MANAGER SPECIFIC FUNCTIONS           */
    /*************************************************/
    private IEnumerator GameWait()
    {
        yield return new WaitForSeconds(2);
    }
    private void SetEntityDefaults()
    {
        EntityManager.Player.AssignBlind(blind.SmallAmount);
        EntityManager.Bot.AssignBlind(blind.BigAmount);
    }
    private void IncreaseRound() 
    {
        if(round == Round.PREFLOP)
            round = Round.FLOP;

        else if(round == Round.FLOP)
            round = Round.TURN;

        else if(round == Round.TURN)
            round = Round.RIVER;

        else if(round == Round.RIVER)
            round = Round.SHOWDOWN;

        else
            round = Round.PREFLOP;
    }
    private void RevealCommunityCards()
    {
        if(round == Round.FLOP)
        {
            communityCard1.sprite = GameManager.Deck.Cards[4].Sprite;
            communityCard2.sprite = GameManager.Deck.Cards[5].Sprite;
            communityCard3.sprite = GameManager.Deck.Cards[6].Sprite;
            communityCard1.enabled = true;
            communityCard2.enabled = true;
            communityCard3.enabled = true;
        }
        else if(round == Round.TURN)
        {
            communityCard4.sprite = GameManager.Deck.Cards[7].Sprite;
            communityCard4.enabled = true;
        }
        else if(round == Round.RIVER)
        {
            communityCard5.sprite = GameManager.Deck.Cards[8].Sprite;
            communityCard5.enabled = true;
        }
        else
            return;
    }
    private void RevealPlayerCards()
    {
        playerCard1.sprite = GameManager.Deck.Cards[0].Sprite;
        playerCard2.sprite = GameManager.Deck.Cards[2].Sprite;
    }
    private void RevealParticipantsCards()
    {
        playerCard1.sprite = GameManager.Deck.Cards[0].Sprite;
        playerCard2.sprite = GameManager.Deck.Cards[2].Sprite;
        opponentCard1.sprite = GameManager.Deck.Cards[1].Sprite;
        opponentCard2.sprite = GameManager.Deck.Cards[3].Sprite;
    }
    private void DisableAllCommunityCards()
    {
        communityCard1.enabled = false;
        communityCard2.enabled = false;
        communityCard3.enabled = false;
        communityCard4.enabled = false;
        communityCard5.enabled = false;
    }
    private void ResetCardSpites()
    {
        playerCard1.sprite = unflippedCard;
        playerCard2.sprite = unflippedCard;
        opponentCard1.sprite = unflippedCard;
        opponentCard2.sprite = unflippedCard;
        communityCard1.sprite = unflippedCard;
        communityCard2.sprite = unflippedCard;
        communityCard3.sprite = unflippedCard;
        communityCard4.sprite = unflippedCard;
        communityCard5.sprite = unflippedCard;
    }
}
