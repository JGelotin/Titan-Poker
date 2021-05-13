using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// To turn into components
// TEMPORARY
public enum Round
{
    PREFLOP,
    FLOP,
    TURN,
    RIVER,
    SHOWDOWN
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public TMP_Text gameResultText;
    public UnityEngine.UI.Button mainMenuButton;

    public TMP_Text playerResult;
    public TMP_Text botResult;
    public TMP_Text playerDecision;
    public TMP_Text botDecision;
    public TMP_Text playerHandRank;
    public TMP_Text playerChipAmount;
    public SpriteRenderer playerChip;
    public TMP_Text playerBetAmount;
    public TMP_Text botHandRank;
    public TMP_Text botChipAmount;
    public SpriteRenderer botChip;
    public TMP_Text botBetAmount;
    public TMP_Text potAmount;

    public UnityEngine.UI.Button checkButton;
    public UnityEngine.UI.Button raiseButton;
    public UnityEngine.UI.Button foldButton;
    public UnityEngine.UI.Button callButton;
    public TMP_InputField raiseAmountText;

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

    private static Deck deck;
    private List<Card> communityCards;
    private static Round round;
    private Blind blind;
    private int gamesPassed;
    private int pot;
    private bool botDecisionMade;
    private bool allIn;

    private Entity player;
    private Entity bot;

    public static Deck Deck { get { return deck; } } 
    public static Round Round { get { return round; } }
    

    private void Start()
    {
        MakeSingleton();
        unflippedCard = Resources.Load<Sprite>("Sprites/Cards/back");

        deck = new Deck();
        communityCards = new List<Card>();
        round = new Round();
        blind = new Blind();
        gamesPassed = 0;
        player = new Entity();
        bot = new Entity();
        botDecisionMade = false;

        StartCoroutine(GameStart());
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
    private void Update()
    {
        if((player.Action == Action.FOLD || bot.Action == Action.FOLD) && botDecisionMade)
        {
            botDecisionMade = false;
            StartCoroutine(ResolveCurrentGame());
            ResetEntityActions();
        }
        else if((player.Action != Action.NONE && bot.Action != Action.NONE) && botDecisionMade)
        {
            if((player.Action == bot.Action && player.Action != Action.RAISE && bot.Action != Action.RAISE) ||
              (player.Action == Action.RAISE && bot.Action == Action.CALL) ||
              (player.Action == Action.CALL && bot.Action == Action.RAISE))
              {
                botDecisionMade = false;

                if(allIn)
                {
                    UIEnableAllCommunityCards();
                    botDecisionMade = true;
                    round = Round.SHOWDOWN;
                }

                if(round == Round.PREFLOP)
                {
                    IncreaseRound();
                    potAmount.text = "POT: " + pot.ToString();
                    ResetEntityActions();
                    StartCoroutine(UpdateNextRoundUI());
                }
                else if(round == Round.FLOP)
                {
                    IncreaseRound();
                    potAmount.text = "POT: " + pot.ToString();
                    ResetEntityActions();
                    StartCoroutine(UpdateNextRoundUI());
                }
                else if(round == Round.TURN)
                {
                    IncreaseRound();
                    potAmount.text = "POT: " + pot.ToString();
                    ResetEntityActions();
                    StartCoroutine(UpdateNextRoundUI());
                }
                else if(round == Round.RIVER)
                {
                    IncreaseRound();
                    botDecisionMade = true;
                }
                else // SHOWDOWN
                {
                    StartCoroutine(ResolveCurrentGame());
                    ResetEntityActions();
                }
              }
        }
    }
    /**************************************/
    /*           MAIN FUNCTIONS           */
    /**************************************/
    private IEnumerator GameStart()
    {
        UIHideAllEntitiesCards();
        UIHideAllEntitiesHandRank();
        UIDisableAllCommunityCards();
        UIDisablePlayerChipsObjects();
        UIDisableBotChipsObjects();
        UIDisableEntityResultText();
        UIDisablePlayerInterfaceObjects();
        playerDecision.text = "";
        botDecision.text = "";
        mainMenuButton.gameObject.SetActive(false);

        gameResultText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        gameResultText.gameObject.SetActive(false);

        StartNewRound();
    }
    private void StartNewRound()
    {
        round = Round.PREFLOP;
        deck.RandomizeDeck();

        deck.deck[0] = new Card(CardSuit.HEART, CardType.SIX);
        deck.deck[2] = new Card(CardSuit.SPADE, CardType.SIX);

        deck.deck[1] = new Card(CardSuit.SPADE, CardType.KING);
        deck.deck[3] = new Card(CardSuit.CLUB, CardType.KING);

        deck.deck[4] = new Card(CardSuit.DIAMOND, CardType.SIX);
        deck.deck[5] = new Card(CardSuit.CLUB, CardType.SIX);
        deck.deck[6] = new Card(CardSuit.HEART, CardType.KING);
        deck.deck[7] = new Card(CardSuit.CLUB, CardType.ACE);
        deck.deck[8] = new Card(CardSuit.HEART, CardType.FIVE);

        UpdateBlinds();
        potAmount.text = "POT: " + pot.ToString();

        UpdateAllEntitiesCards();
        UIUpdateEntitiesChipAmount();
        UpdateCommunityCards();
        UIDisableAllCommunityCards();
        UIDisablePlayerChipsObjects();
        UIDisableBotChipsObjects();
        UIDisableEntityResultText();
        UIDefaultPlayerInterfaceObjects();
        raiseAmountText.gameObject.SetActive(false);
        UIResetEntitiesDecision();
        UIShowPlayerCards();
        UpdateAllEntitiesHandRank();
        UIUpdateEntitiesHandRank();
        UIShowAllEntitiesHandRank();
        UIEnablePlayerInterfaceObjects();
        UIHideOpponentHandRank();
    }
    private IEnumerator ResolveCurrentGame()
    {
        int gameResult = HandCompare.Compare(player.Hand, bot.Hand);

        if(player.Action == Action.FOLD || bot.Action == Action.FOLD)
        {
            if(player.Action == Action.FOLD)
            {
                Debug.Log("Bot given: " + pot);
                bot.AddChipAmount(pot);
                UIUpdateEntitiesChipAmount();

                UIEnableEntityResultText();

                playerResult.text = "LOSS";
                botResult.text = "WIN";                
            }
            else
            {
                Debug.Log("Player given: " + pot);
                player.AddChipAmount(pot);
                UIUpdateEntitiesChipAmount();

                UIEnableEntityResultText();

                playerResult.text = "WIN";
                botResult.text = "LOSS";
            }
            UIShowAllEntitesCards();
            UIShowAllEntitiesHandRank();
        }
        else
        {
            UIShowAllEntitesCards();
            UIShowAllEntitiesHandRank();
            if(gameResult == 0)
            {
                UIEnableEntityResultText();

                playerResult.text = "WIN";
                botResult.text = "LOSS";

                player.AddChipAmount(pot);
                UIUpdateEntitiesChipAmount();
            }
            else if(gameResult == 1)
            {
                UIEnableEntityResultText();

                playerResult.text = "DRAW";
                botResult.text = "DRAW";

                player.AddChipAmount(pot / 2);
                bot.AddChipAmount(pot / 2);

                UIUpdateEntitiesChipAmount();
            }
            else
            {
                UIEnableEntityResultText();

                playerResult.text = "LOSS";
                botResult.text = "WIN";

                bot.AddChipAmount(pot);
                UIUpdateEntitiesChipAmount();
            }
        }
        
        if((player.Action != Action.FOLD && bot.Action != Action.FOLD) && allIn == true)
        {
            if(gameResult == 0 || gameResult == 2)
            {
                yield return new WaitForSeconds(5);
                GameEnd();
            }
            else
            {
                yield return new WaitForSeconds(5);
                UIHideAllEntitiesCards();
                UIHideAllEntitiesHandRank();
                UIShowPlayerCards();
                UIShowAllEntitiesHandRank();
                UIHideOpponentHandRank();
                UIEnableAllCommunityCards();
                pot = 0;
                potAmount.text = "POT: " + pot.ToString();
                gamesPassed++;
                allIn = false;
                StartNewRound();
            }
        }
        else
        {
            yield return new WaitForSeconds(5);
            UIHideAllEntitiesCards();
            UIHideAllEntitiesHandRank();
            UIShowPlayerCards();
            UIShowAllEntitiesHandRank();
            UIHideOpponentHandRank();
            pot = 0;
            potAmount.text = "POT: " + pot.ToString();
            gamesPassed++;
            StartNewRound();
        }
    }
    private void BotTurn()
    {
        GenerateBotDecision();

        Debug.Log("BOT DECISION GENERATED");

        if(bot.BetAmount != 0)
        {
            Debug.Log(bot.BetAmount);
            UIEnableBotChipsObjects();
            UIUpdateBotChipObjects();
        }
    }
    private IEnumerator UpdateNextRoundUI()
    {
        yield return new WaitForSeconds(2);
        UIDefaultPlayerInterfaceObjects();
        UIResetAllChipObjects();
        UIUpdateCommunityCards();
        UpdateAllEntitiesHandRank();
        UIUpdateEntitiesHandRank();
        UIDefaultPlayerInterfaceObjects();
        UIResetEntitiesDecision();
    }
    private void GameEnd()
    {
        UIHideAllEntitiesCards();
        UIHideAllEntitiesHandRank();
        UIDisableAllCommunityCards();
        UIDisablePlayerChipsObjects();
        UIDisableBotChipsObjects();
        UIDisableEntityResultText();
        UIDisablePlayerInterfaceObjects();
        playerDecision.text = "";
        botDecision.text = "";

        if(player.ChipAmount == 0)
        {
            gameResultText.gameObject.SetActive(true);
            gameResultText.text = "YOU LOST";
        }
        else
        {
            gameResultText.gameObject.SetActive(true);
            gameResultText.text = "YOU WON!";
        }

        mainMenuButton.gameObject.SetActive(true);
        gamesPassed = 0;
        UpdateBlinds();
        player = new Entity();
        bot = new Entity();
        botDecisionMade = false;
        allIn = false;
    }
    /*************************************/
    /*         UPDATE FUNCTIONS          */
    /*************************************/
    private void UpdateAllEntitiesHandRank()
    {
        if(round == Round.PREFLOP)
        {
            List<Card> temp = new List<Card>();
            player.UpdateHand(temp);
            bot.UpdateHand(temp);
        }
        else if(round == Round.FLOP)
        {
            List<Card> temp = new List<Card>();
            temp.Add(deck.Cards[4]);
            temp.Add(deck.Cards[5]);
            temp.Add(deck.Cards[6]);
            player.UpdateHand(temp);
            bot.UpdateHand(temp);
        }
        else if(round == Round.TURN)
        {
            List<Card> temp = new List<Card>();
            temp.Add(deck.Cards[4]);
            temp.Add(deck.Cards[5]);
            temp.Add(deck.Cards[6]);
            temp.Add(deck.Cards[7]);
            player.UpdateHand(temp);
            bot.UpdateHand(temp);
        }
        else if(round == Round.RIVER)
        {
            List<Card> temp = new List<Card>();
            temp.Add(deck.Cards[4]);
            temp.Add(deck.Cards[5]);
            temp.Add(deck.Cards[6]);
            temp.Add(deck.Cards[7]);
            temp.Add(deck.Cards[8]);
            player.UpdateHand(temp);
            bot.UpdateHand(temp);
        }
        else
            return;
    }
    private void UpdateAllEntitiesCards()
    {
        player.SetHoleCards(deck.Cards[0], deck.Cards[2]);
        bot.SetHoleCards(deck.Cards[1], deck.Cards[3]);
    }
    private void UpdateCommunityCards()
    {
        communityCards.Add(deck.Cards[4]);
        communityCards.Add(deck.Cards[5]);
        communityCards.Add(deck.Cards[6]);
        communityCards.Add(deck.Cards[7]);
        communityCards.Add(deck.Cards[8]);
    }
    private void UpdateBlinds()
    {
        int multiplier = 1 + (gamesPassed / 5);
        Debug.Log("MULTIPLIER: " + multiplier);
        blind.IncreaseBlind(multiplier);

        if(gamesPassed % 2 == 0)
        {
            player.SetBlind(blind.SmallAmount);
            bot.SetBlind(blind.BigAmount);
        }
        else
        {
            player.SetBlind(blind.BigAmount);
            bot.SetBlind(blind.SmallAmount);
        }

        player.SubtractChipAmount(player.BlindAmount);
        bot.SubtractChipAmount(bot.BlindAmount);
        pot += player.BlindAmount;
        pot += bot.BlindAmount;
    }

    /*************************************/
    /*          ROUND FUNCTIONS          */
    /*************************************/
    public static void IncreaseRound() 
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
    private void ResetRound()
    {
        round = Round.PREFLOP;
    }
    /*************************************/
    /*           PLAYER INPUT            */
    /*************************************/
    public void CheckButtonClick()
    {
        player.SetAction(Action.CHECK);
        UIUpdateEntitiesDecision();
        StartCoroutine(BotAction());
    }
    public void RaiseButtonClick()
    {
        if(player.Action == Action.NONE || player.Action == Action.CHECK)
        {
            player.SetAction(Action.PRERAISE);
            checkButton.gameObject.SetActive(false);
            foldButton.gameObject.SetActive(false);
            callButton.gameObject.SetActive(false);
            raiseAmountText.gameObject.SetActive(true);
            raiseButton.transform.localPosition = new Vector3((float)-419.9, (float)-385.6, 0);
        }
        else if(player.Action == Action.PRERAISE)
        {
            player.SetAction(Action.RAISE);
            Debug.Log("BET: " + Int32.Parse(raiseAmountText.text));
            player.SetBetAmount(Int32.Parse(raiseAmountText.text));
            pot += player.BetAmount;
            potAmount.text = "POT: " + pot.ToString();
            UIUpdatePlayerChipObjects();
            UIUpdateEntitiesChipAmount();

            if(player.ChipAmount == 0)
            {
                allIn = true;
                playerDecision.text = "ALL IN";
            }

            checkButton.gameObject.SetActive(true);
            foldButton.gameObject.SetActive(true);
            callButton.gameObject.SetActive(true);
            raiseAmountText.gameObject.SetActive(false);
            raiseButton.transform.localPosition = new Vector3((float)-119.09, (float)-385.6, 0);
            
            UIUpdateEntitiesDecision();
            StartCoroutine(BotAction());
        }
    }
    public void CallButtonClick()
    {
        player.SetAction(Action.CALL);
        player.SetBetAmount(bot.BetAmount);
        pot += player.BetAmount;
        potAmount.text = "POT: " + pot.ToString();
        UIUpdatePlayerChipObjects();
        UIUpdateEntitiesChipAmount();
        UIUpdateEntitiesDecision();
        UIDisablePlayerInterfaceObjects();

        if(player.ChipAmount == 0)
        {
            allIn = true;
            playerDecision.text = "ALL IN";
        }
        if(bot.Action == Action.RAISE)
        {
            botDecisionMade = true;
        }
    }
    public void FoldButtonClick()
    {
        botDecisionMade = true;
        player.SetAction(Action.FOLD);
        UIUpdateEntitiesDecision();
        UIDisablePlayerInterfaceObjects();
    }
    private IEnumerator BotAction()
    {
        UIDisablePlayerInterfaceObjects();
        yield return new WaitForSeconds(2);
        GenerateBotDecision();

        if(bot.Action == Action.FOLD && allIn == true)
        {
            allIn = false;
        }
        if(bot.Action == Action.RAISE || bot.Action == Action.CALL)
        {
            Debug.Log("BOT BET AMOUNT: " + bot.BetAmount);
            pot += bot.BetAmount;
            potAmount.text = "POT: " + pot.ToString();
            UIEnableBotChipsObjects();
            UIUpdateBotChipObjects();

            if(bot.ChipAmount == 0)
            {
                allIn = true;
                botDecision.text = "ALL IN";
            }
        }
        UIUpdateEntitiesDecision();
        UIUpdateEntitiesChipAmount();

        if(bot.Action == Action.RAISE)
        {
            yield return new WaitForSeconds(2);
            UIEnablePlayerInterfaceObjects();
        }
        else
        {
            botDecisionMade = true;
        }
        Debug.Log(bot.Action);
    }
    public void MainMenuButtonClick()
    {
        instance = null;
        SceneManager.LoadScene("MainMenu");
    }

    /*************************************/
    /*        ENTITY MODIFICATION        */
    /*************************************/
    private void ResetEntityActions()
    {
        player.SetAction(Action.NONE);
        bot.SetAction(Action.NONE);
    }
    /*********************************************/
    /*        USER INTERFACE MODIFICATION        */
    /*********************************************/
    private void UIUpdateEntitiesHandRank()
    {
        playerHandRank.text = player.Hand.Rank.Name;
        
        // DEBUG PURPOSES
        botHandRank.text = bot.Hand.Rank.Name;
    }
    private void UIUpdateEntitiesDecision()
    {
        if(player.Action == Action.NONE)
            playerDecision.text = "";
        else if(player.Action == Action.CHECK)
            playerDecision.text = "CHECK";
        else if(player.Action == Action.RAISE)
            playerDecision.text = "RAISE";
        else if(player.Action == Action.CALL)
            playerDecision.text = "CALL";
        else
            playerDecision.text = "FOLD";

        if(bot.Action == Action.NONE)
            botDecision.text = "";
        else if(bot.Action == Action.CHECK)
            botDecision.text = "CHECK";
        else if(bot.Action == Action.RAISE)
            botDecision.text = "RAISE";
        else if(bot.Action == Action.CALL)
            botDecision.text = "CALL";
        else
            botDecision.text = "FOLD";
    }
    private void UIResetEntitiesDecision()
    {
        playerDecision.text = "";
        botDecision.text = "";
    }
    private void UIUpdateEntitiesChipAmount()
    {
        playerChipAmount.text = player.ChipAmount.ToString();
        botChipAmount.text = bot.ChipAmount.ToString();
    }
    private void UIEnablePlayerChipsObjects()
    {
        playerChip.gameObject.SetActive(true);
        playerBetAmount.gameObject.SetActive(true);
    }
    private void UIEnableBotChipsObjects()
    {
        botChip.gameObject.SetActive(true);
        botBetAmount.gameObject.SetActive(true);
    }
    private void UIDisablePlayerChipsObjects()
    {
        playerChip.gameObject.SetActive(false);
        playerBetAmount.gameObject.SetActive(false);
    }
    private void UIDisableBotChipsObjects()
    {
        botChip.gameObject.SetActive(false);
        botBetAmount.gameObject.SetActive(false);
    }
    private void UIUpdatePlayerChipObjects()
    {
        playerChip.gameObject.SetActive(true);
        playerBetAmount.gameObject.SetActive(true);
        playerBetAmount.text = player.BetAmount.ToString();
        
        if(player.BetAmount < 2)
            playerChip.sprite = Resources.Load<Sprite>("Sprites/Chips/individual_pngs/1");
        else if(player.BetAmount < 5)
            playerChip.sprite = Resources.Load<Sprite>("Sprites/Chips/individual_pngs/2");
        else if(player.BetAmount < 10)
            playerChip.sprite = Resources.Load<Sprite>("Sprites/Chips/individual_pngs/5");
        else if(player.BetAmount < 20)
            playerChip.sprite = Resources.Load<Sprite>("Sprites/Chips/individual_pngs/10");
        else if(player.BetAmount < 50)
            playerChip.sprite = Resources.Load<Sprite>("Sprites/Chips/individual_pngs/20");
        else if(player.BetAmount < 100)
            playerChip.sprite = Resources.Load<Sprite>("Sprites/Chips/individual_pngs/50");
        else if(player.BetAmount < 250)
            playerChip.sprite = Resources.Load<Sprite>("Sprites/Chips/individual_pngs/100");
        else if(player.BetAmount < 500)
            playerChip.sprite = Resources.Load<Sprite>("Sprites/Chips/individual_pngs/250");
        else if(player.BetAmount < 1000)
            playerChip.sprite = Resources.Load<Sprite>("Sprites/Chips/individual_pngs/500");
        else
            playerChip.sprite = Resources.Load<Sprite>("Sprites/Chips/individual_pngs/1000");
    }
    private void UIUpdateBotChipObjects()
    {
        botChip.gameObject.SetActive(true);
        botBetAmount.gameObject.SetActive(true);
        botBetAmount.text = bot.BetAmount.ToString();

        if(bot.BetAmount < 2)
            botChip.sprite = Resources.Load<Sprite>("Sprites/Chips/individual_pngs/1");
        else if(bot.BetAmount < 5)
            botChip.sprite = Resources.Load<Sprite>("Sprites/Chips/individual_pngs/2");
        else if(bot.BetAmount < 10)
            botChip.sprite = Resources.Load<Sprite>("Sprites/Chips/individual_pngs/5");
        else if(bot.BetAmount < 20)
            botChip.sprite = Resources.Load<Sprite>("Sprites/Chips/individual_pngs/10");
        else if(bot.BetAmount < 50)
            botChip.sprite = Resources.Load<Sprite>("Sprites/Chips/individual_pngs/20");
        else if(bot.BetAmount < 100)
            botChip.sprite = Resources.Load<Sprite>("Sprites/Chips/individual_pngs/50");
        else if(bot.BetAmount < 250)
            botChip.sprite = Resources.Load<Sprite>("Sprites/Chips/individual_pngs/100");
        else if(bot.BetAmount < 500)
            botChip.sprite = Resources.Load<Sprite>("Sprites/Chips/individual_pngs/250");
        else if(bot.BetAmount < 1000)
            botChip.sprite = Resources.Load<Sprite>("Sprites/Chips/individual_pngs/500");
        else
            botChip.sprite = Resources.Load<Sprite>("Sprites/Chips/individual_pngs/1000");
    }
    private void UIResetAllChipObjects()
    {
        playerChip.gameObject.SetActive(false);
        botChip.gameObject.SetActive(false);
        playerBetAmount.text = "";
        botBetAmount.text = "";
        playerBetAmount.gameObject.SetActive(false);
        botBetAmount.gameObject.SetActive(false);
    }

    /************* CARD SPRITES **************/
    private void UIUpdateCommunityCards()
    {
        if(round == Round.FLOP)
        {
            communityCard1.sprite = deck.Cards[4].Sprite;
            communityCard2.sprite = deck.Cards[5].Sprite;
            communityCard3.sprite = deck.Cards[6].Sprite;
            communityCard1.enabled = true;
            communityCard2.enabled = true;
            communityCard3.enabled = true;
        }
        else if(round == Round.TURN)
        {
            communityCard4.sprite = deck.Cards[7].Sprite;
            communityCard4.enabled = true;
        }
        else if(round == Round.RIVER)
        {
            communityCard5.sprite = deck.Cards[8].Sprite;
            communityCard5.enabled = true;
        }
        else
            return;
    }
    private void UIShowPlayerCards()
    {
        playerCard1.sprite = deck.Cards[0].Sprite;
        playerCard2.sprite = deck.Cards[2].Sprite;
    }
    private void UIHideAllEntitiesCards()
    {
        playerCard1.sprite = unflippedCard;
        playerCard2.sprite = unflippedCard;
        opponentCard1.sprite = unflippedCard;
        opponentCard2.sprite = unflippedCard;
    }
    private void UIShowAllEntitesCards()
    {
        playerCard1.sprite = deck.Cards[0].Sprite;
        playerCard2.sprite = deck.Cards[2].Sprite;
        opponentCard1.sprite = deck.Cards[1].Sprite;
        opponentCard2.sprite = deck.Cards[3].Sprite;
    }
    private void UIShowAllEntitiesHandRank()
    {
        playerHandRank.gameObject.SetActive(true);
        botHandRank.gameObject.SetActive(true);
    }
    private void UIHideAllEntitiesHandRank()
    {
        playerHandRank.gameObject.SetActive(false);
        botHandRank.gameObject.SetActive(false);
    }
    private void UIHideOpponentHandRank()
    {
        botHandRank.gameObject.SetActive(false);
    }
    private void UIEnableAllCommunityCards()
    {
        communityCard1.enabled = true;
        communityCard2.enabled = true;
        communityCard3.enabled = true;
        communityCard4.enabled = true;
        communityCard5.enabled = true;
        communityCard1.sprite = deck.Cards[4].Sprite;
        communityCard2.sprite = deck.Cards[5].Sprite;
        communityCard3.sprite = deck.Cards[6].Sprite;
        communityCard4.sprite = deck.Cards[7].Sprite;
        communityCard5.sprite = deck.Cards[8].Sprite;
    }
    private void UIDisableAllCommunityCards()
    {
        communityCard1.enabled = false;
        communityCard2.enabled = false;
        communityCard3.enabled = false;
        communityCard4.enabled = false;
        communityCard5.enabled = false;
    }

    /************* PLAYER ACTION PRESETS **************/
    private void UIDisablePlayerInterfaceObjects()
    {
        checkButton.gameObject.SetActive(false);
        raiseButton.gameObject.SetActive(false);
        callButton.gameObject.SetActive(false);
        foldButton.gameObject.SetActive(false);
        raiseAmountText.text = "";
        raiseAmountText.gameObject.SetActive(false);
    }
    private void UIEnablePlayerInterfaceObjects()
    {
        if(bot.Action == Action.NONE || bot.Action == Action.CHECK)
        {
            checkButton.gameObject.SetActive(true);
            raiseButton.gameObject.SetActive(true);
            callButton.gameObject.SetActive(true);
            foldButton.gameObject.SetActive(true);

            checkButton.enabled = true;
            checkButton.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            callButton.enabled = false;
            callButton.GetComponent<UnityEngine.UI.Image>().color = Color.gray;
        }
        else if(bot.Action == Action.RAISE)
        {
            checkButton.gameObject.SetActive(true);
            raiseButton.gameObject.SetActive(true);
            callButton.gameObject.SetActive(true);
            foldButton.gameObject.SetActive(true);

            checkButton.enabled = false;
            checkButton.GetComponent<UnityEngine.UI.Image>().color = Color.gray;
            callButton.enabled = true;
            callButton.GetComponent<UnityEngine.UI.Image>().color = Color.white;
        }
        else
            return;
    }
    private void UIDefaultPlayerInterfaceObjects()
    {
        checkButton.gameObject.SetActive(true);
        raiseButton.gameObject.SetActive(true);
        callButton.gameObject.SetActive(true);
        foldButton.gameObject.SetActive(true);
        checkButton.enabled = true;
        checkButton.GetComponent<UnityEngine.UI.Image>().color = Color.white;
        callButton.enabled = false;
        callButton.GetComponent<UnityEngine.UI.Image>().color = Color.gray;
    }
    private void UIEnableEntityResultText()
    {
        playerResult.gameObject.SetActive(true);
        botResult.gameObject.SetActive(true);
    }
    private void UIDisableEntityResultText()
    {
        playerResult.gameObject.SetActive(false);
        botResult.gameObject.SetActive(false);
    }

    /*********************************************/
    /*           GENERATE BOT DECISION           */
    /*********************************************/
    private void GenerateBotDecision()
    {
        System.Random rand = new System.Random();

        int decision = rand.Next(1, 100);
        if(player.Action == Action.NONE)
            GenerateBotDecisionIfPlayerNoAction(decision);

        else if(player.Action == Action.CHECK)
            GenerateBotDecisionIfPlayerChecks(decision);

        else if(player.Action == Action.RAISE)
            GenerateBotDecisionIfPlayerRaises(decision);

        else
            return;

    }
    private void GenerateBotDecisionIfPlayerNoAction(int decision)
    {
        if(bot.Hand.Rank.Value == 1)
            bot.SetAction(Action.CHECK);
        
        else if(bot.Hand.Rank.Value <= 3)
        {
            if(decision == 1)
            {
                bot.SetAction(Action.RAISE);
                bot.SetBetAmount(bot.ChipAmount);
            }
            else if(decision < 13)
                bot.SetAction(Action.CHECK);

            else if(decision < 40)
            {
                bot.SetAction(Action.RAISE);

                if(bot.ChipAmount <= blind.BigAmount * 2)
                    bot.SetBetAmount(bot.ChipAmount);
                else
                    bot.SetBetAmount(blind.BigAmount);
            }
            else
            {
                bot.SetAction(Action.RAISE);

                if(bot.ChipAmount <= blind.BigAmount * 2)
                    bot.SetBetAmount(bot.ChipAmount);
                else
                    bot.SetBetAmount(blind.BigAmount * 2);
            }
        }
        else if(bot.Hand.Rank.Value <= 6)
        {
            if(decision < 4)
                bot.SetAction(Action.CHECK);

            else if(decision < 20)
            {
                bot.SetAction(Action.RAISE);

                if(bot.ChipAmount <= blind.BigAmount * 2)
                    bot.SetBetAmount(bot.ChipAmount);
                else
                    bot.SetBetAmount(blind.BigAmount);
            }
            else if(decision < 90)
            {
                bot.SetAction(Action.RAISE);

                if(bot.ChipAmount <= blind.BigAmount * 2)
                    bot.SetBetAmount(bot.ChipAmount);
                else
                    bot.SetBetAmount(blind.BigAmount * 2);
            }
            else
            {
                bot.SetAction(Action.RAISE);
                bot.SetBetAmount(bot.ChipAmount);
            }
        }
        else
        {
            if(decision == 1)
            {
                bot.SetAction(Action.CHECK);
            }
            else if(decision < 16)
            {
                bot.SetAction(Action.RAISE);

                if(bot.ChipAmount <= blind.BigAmount * 2)
                    bot.SetBetAmount(bot.ChipAmount);
                else
                    bot.SetBetAmount(blind.BigAmount * 2);
            }
            else
            {
                bot.SetAction(Action.RAISE);
                bot.SetBetAmount(bot.ChipAmount);
            }
        }
    }
    private void GenerateBotDecisionIfPlayerChecks(int decision)
    {
        if(bot.Hand.Rank.Value == 1)
            bot.SetAction(Action.CHECK);
        
        else if(bot.Hand.Rank.Value <= 3)
        {
            if(decision <= 4)
            {
                bot.SetAction(Action.RAISE);
                bot.SetBetAmount(bot.ChipAmount);
            }
            else if(decision < 15)
                bot.SetAction(Action.CHECK);

            else if(decision < 50)
            {
                bot.SetAction(Action.RAISE);

                if(bot.ChipAmount <= blind.BigAmount * 2)
                    bot.SetBetAmount(bot.ChipAmount);
                else
                    bot.SetBetAmount(blind.BigAmount);
            }
            else
            {
                bot.SetAction(Action.RAISE);

                if(bot.ChipAmount <= blind.BigAmount * 2)
                    bot.SetBetAmount(bot.ChipAmount);
                else
                    bot.SetBetAmount(blind.BigAmount * 2);
            }
        }
        else if(bot.Hand.Rank.Value <= 6)
        {
            if(decision == 1)
                bot.SetAction(Action.CHECK);

            else if(decision < 20)
            {
                bot.SetAction(Action.RAISE);

                if(bot.ChipAmount <= blind.BigAmount * 2)
                    bot.SetBetAmount(bot.ChipAmount);
                else
                    bot.SetBetAmount(blind.BigAmount);
            }
            else if(decision < 90)
            {
                bot.SetAction(Action.RAISE);

                if(bot.ChipAmount <= blind.BigAmount * 2)
                    bot.SetBetAmount(bot.ChipAmount);
                else
                    bot.SetBetAmount(blind.BigAmount * 2);
            }
            else
            {
                bot.SetAction(Action.RAISE);
                bot.SetBetAmount(bot.ChipAmount);
            }
        }
        else
        {
            if(decision < 40)
            {
                bot.SetAction(Action.RAISE);

                if(bot.ChipAmount <= blind.BigAmount * 2)
                    bot.SetBetAmount(bot.ChipAmount);
                else
                    bot.SetBetAmount(blind.BigAmount * 2);
            }
            else
            {
                bot.SetAction(Action.RAISE);
                bot.SetBetAmount(bot.ChipAmount);
            }
        }
    }
    private void GenerateBotDecisionIfPlayerRaises(int decision)
    {
        if(player.BetAmount <= blind.BigAmount * 2)
        {
            if(bot.Hand.Rank.Value <= 2)
            {
                if(decision < 20)
                {
                    if(player.BetAmount >= bot.ChipAmount)
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                    else
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(player.BetAmount);
                    }
                }
                else if(decision < 75)
                {
                    if(player.BetAmount >= bot.ChipAmount)
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                    else
                    {
                        bot.SetAction(Action.RAISE);
                        bot.SetBetAmount(player.BetAmount + blind.BigAmount);
                    }
                }
                else
                {
                    bot.SetAction(Action.FOLD);
                }
            }
            else if(bot.Hand.Rank.Value <= 4)
            {
                if(decision < 30)
                {
                    if(player.BetAmount >= bot.ChipAmount)
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                    else
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(player.BetAmount);
                    }
                }
                else if(decision < 95)
                {
                    if(player.BetAmount >= bot.ChipAmount)
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                    else
                    {
                        bot.SetAction(Action.RAISE);
                        bot.SetBetAmount(player.BetAmount + blind.BigAmount);
                    }
                }
                else
                {
                    bot.SetAction(Action.FOLD);
                }
            }
            else if(bot.Hand.Rank.Value <= 6)
            {
                if(decision < 45)
                {
                    if(player.BetAmount >= bot.ChipAmount)
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                    else
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(player.BetAmount);
                    }
                }
                else if(decision < 98)
                {
                    if(player.BetAmount >= bot.ChipAmount)
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                    else
                    {
                        bot.SetAction(Action.RAISE);
                        bot.SetBetAmount(player.BetAmount + blind.BigAmount);
                    }
                }
                else
                {
                    if(player.BetAmount >= bot.ChipAmount)
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                    else
                    {
                        bot.SetAction(Action.RAISE);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                }
            }
            else if(bot.Hand.Rank.Value <= 8)
            {
                if(decision < 4)
                {
                    if(player.BetAmount >= bot.ChipAmount)
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                    else
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(player.BetAmount);
                    }
                }
                else if(decision < 80)
                {
                    if(player.BetAmount >= bot.ChipAmount)
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                    else
                    {
                        bot.SetAction(Action.RAISE);
                        bot.SetBetAmount(player.BetAmount + blind.BigAmount);
                    }
                }
                else
                {
                    if(player.BetAmount >= bot.ChipAmount)
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                    else
                    {
                        bot.SetAction(Action.RAISE);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                }
            }
            else
            {
                if(decision < 50)
                {
                    if(player.BetAmount >= bot.ChipAmount)
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                    else
                    {
                        bot.SetAction(Action.RAISE);
                        bot.SetBetAmount(player.BetAmount + blind.BigAmount);
                    }
                }
                else
                {
                    if(player.BetAmount >= bot.ChipAmount)
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                    else
                    {
                        bot.SetAction(Action.RAISE);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                }
            }
        }
        else if(player.BetAmount <= blind.BigAmount * 4)
        {
            if(bot.Hand.Rank.Value <= 2)
            {
                if(decision < 5)
                {
                    if(player.BetAmount >= bot.ChipAmount)
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                    else
                    {
                        bot.SetAction(Action.RAISE);
                        bot.SetBetAmount(player.BetAmount + blind.BigAmount);
                    }
                }
                else if(decision < 20)
                {
                    if(player.BetAmount >= bot.ChipAmount)
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                    else
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(player.BetAmount);
                    }
                }
                else
                {
                    bot.SetAction(Action.FOLD);
                }
            }
            else if(bot.Hand.Rank.Value <= 4)
            {
                if(decision < 15)
                {
                    if(player.BetAmount >= bot.ChipAmount)
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                    else
                    {
                        bot.SetAction(Action.RAISE);
                        bot.SetBetAmount(player.BetAmount + blind.BigAmount);
                    }
                }
                else if(decision < 60)
                {
                    if(player.BetAmount >= bot.ChipAmount)
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                    else
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(player.BetAmount);
                    }
                }
                else
                {
                    bot.SetAction(Action.FOLD);
                }
            }
            else if(bot.Hand.Rank.Value <= 6)
            {
                if(decision < 40)
                {
                    if(player.BetAmount >= bot.ChipAmount)
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                    else
                    {
                        bot.SetAction(Action.RAISE);
                        bot.SetBetAmount(player.BetAmount + blind.BigAmount);
                    }
                }
                else if(decision < 85)
                {
                    if(player.BetAmount >= bot.ChipAmount)
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                    else
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(player.BetAmount);
                    }
                }
                else
                {
                    bot.SetAction(Action.FOLD);
                }
            }
            else if(bot.Hand.Rank.Value <= 8)
            {
                if(decision < 60)
                {
                    if(player.BetAmount >= bot.ChipAmount)
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                    else
                    {
                        bot.SetAction(Action.RAISE);
                        bot.SetBetAmount(player.BetAmount + blind.BigAmount);
                    }
                }
                else if(decision < 90)
                {
                    if(player.BetAmount >= bot.ChipAmount)
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                    else
                    {
                        bot.SetAction(Action.RAISE);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                }
                else
                {
                    if(player.BetAmount >= bot.ChipAmount)
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                    else
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(player.BetAmount);
                    }
                }
            }
            else
            {
                if(decision < 20)
                {
                    if(player.BetAmount >= bot.ChipAmount)
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                    else
                    {
                        bot.SetAction(Action.RAISE);
                        bot.SetBetAmount(player.BetAmount + blind.BigAmount);
                    }
                }
                else
                {
                    if(player.BetAmount >= bot.ChipAmount)
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                    else
                    {
                        bot.SetAction(Action.RAISE);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                }
            }
        }
        else
        {
            if(bot.Hand.Rank.Value <= 2)
            {
                if(decision < 10)
                {
                    if(player.BetAmount >= bot.ChipAmount)
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                    else
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(player.BetAmount);
                    }
                }
                else
                {
                    bot.SetAction(Action.FOLD);
                }
            }
            else if(bot.Hand.Rank.Value <= 4)
            {
                if(decision < 40)
                {
                    if(player.BetAmount >= bot.ChipAmount)
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                    else
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(player.BetAmount);
                    }
                }
                else
                {
                    bot.SetAction(Action.FOLD);
                }
            }
            else if(bot.Hand.Rank.Value <= 6)
            {
                if(decision < 20)
                {
                    if(player.BetAmount >= bot.ChipAmount)
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                    else
                    {
                        bot.SetAction(Action.RAISE);
                        bot.SetBetAmount(player.BetAmount + blind.BigAmount);
                    }
                }
                else if(decision < 90)
                {
                    if(player.BetAmount >= bot.ChipAmount)
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                    else
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(player.BetAmount);
                    }
                }
                else
                {
                    bot.SetAction(Action.FOLD);
                }
            }
            else if(bot.Hand.Rank.Value <= 8)
            {
                if(decision < 40)
                {
                    if(player.BetAmount >= bot.ChipAmount)
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                    else
                    {
                        bot.SetAction(Action.RAISE);
                        bot.SetBetAmount(player.BetAmount + blind.BigAmount);
                    }
                }
                else if(decision < 97)
                {
                    if(player.BetAmount >= bot.ChipAmount)
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                    else
                    {
                        bot.SetAction(Action.RAISE);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                }
                else
                {
                    if(player.BetAmount >= bot.ChipAmount)
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                    else
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(player.BetAmount);
                    }
                }
            }
            else
            {
                if(decision < 30)
                {
                    if(player.BetAmount >= bot.ChipAmount)
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                    else
                    {
                        bot.SetAction(Action.RAISE);
                        bot.SetBetAmount(player.BetAmount + blind.BigAmount);
                    }
                }
                else
                {
                    if(player.BetAmount >= bot.ChipAmount)
                    {
                        bot.SetAction(Action.CALL);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                    else
                    {
                        bot.SetAction(Action.RAISE);
                        bot.SetBetAmount(bot.ChipAmount);
                    }
                }
            }
        }
    }
}
