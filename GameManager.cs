using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Components")]
    public PlayingCard[] deck;
    public ComCardManager comCardManager;
    public PlayerManager playerManager;
    public DealerManager dealerManager;
    public StackCounter playerStack;
    public StackCounter dealerStack;
    public PotCounter potCounter; 
    public UIManager uiManager;

    [Header("Int Switches")]
    public int playerDealSwitch = 0;
    public int gameStage = 0;

    [Header("Game Values")]
    public int playerOneHandValue;
    public int playerTwoHandValue;
    public int playerOneHighCard;
    public int playerTwoHighCard;
    public int potSize;
    

    [Header("Triggers")]
    #region "TS Triggers"
    public bool dealCard;
    public bool resetCardsStatus;
    public bool resetHoleCards;
    public bool dealFlop;
    public bool dealTurn;
    public bool dealRiver;
    public bool resetComCardSlots;
    #endregion
    #region "Game Triggers"

    public bool aiBetReady = false;
    public bool playerBetReady = false;
    public bool aiMadeBet = false;
    public bool playerMadeBet = false;
    private bool doneDealing = false;


    public bool playerAllIn = false;
    public bool playerFolds = false;
    public bool playerCall = false;
    

    public bool AiAllIn = false; 
    public bool aiFolds = false;
    public bool aiCall = false;
    public bool aiRaise = false;

    public bool gameReady = false;
    public bool riverDealt = false;

    public bool playerBusted = false;   
    public bool aiBusted = false;

    public bool gotWinner = false;
    public bool resetting = false;
    public bool stopDealing = false;

    public bool bigPause = false;
    public int firstCheck = 0;
    public int lastCheck = 0;
    public int playerLastBet = 0;
    #endregion

    public void Start()
    {      
        comCardManager = FindFirstObjectByType<ComCardManager>();
        uiManager = FindFirstObjectByType<UIManager>();
        potCounter = FindFirstObjectByType<PotCounter>();
        MassReset();
        StartCoroutine(WaitOnStart());
        CloseBettingAction();

    }

    public void Update()
    {
        playerLastBet = playerManager.lastBet;
        #region // check for bust
        if (gameStage > 0 && playerManager.stack == 0)
        {
            playerAllIn = true;
        }
        else if (gameStage > 0 && dealerManager.aiStack == 0)
        {
            AiAllIn = true;
        }
        else if (playerManager.cardSlot[0].cardData == null && playerManager.stack == 0)
        {
            playerBusted = true;
            PlayerBust();
            return;
        }
        else if (dealerManager.cardSlot[0].cardData == null && dealerManager.aiStack == 0)
        {
            aiBusted = true;
            DealerBust();
            return;
        }
        #endregion

        potSize = potCounter.pot;
        //Debug.Log(potSize.ToString());
        if (playerBetReady && aiRaise && doneDealing && !playerAllIn)
        {
            aiRaise = false;
            StartBettingAction();
        }

        if (playerBetReady && aiBetReady && doneDealing && !playerAllIn)
        {
            //if (gameStage == 0)
            //gameStage++;
            StartBettingAction();
        }
        else if (playerMadeBet && !playerAllIn)
        {
            if (!AiAllIn)
            {
                dealerManager.getAiBet(playerManager.lastBet);
                gameReady = true;
                if (!aiRaise)
                {
                    gameStage++;
                    playerMadeBet = false;
                   
                }
                else
                {
                    playerMadeBet = false;
                    playerBetReady = true;
                }
            }
            else if (AiAllIn)
            {
                gameStage++;
                playerMadeBet = false;
            }
        }
        else if (gameReady)
        {
            CloseBettingAction();
        }
        if (playerAllIn && !playerBusted && !aiBusted)
        {
            CloseBettingAction();
            if (!AiAllIn)
            {
                if (firstCheck == 0)
                {
                    dealerManager.getAiBet(playerManager.lastBet);
                    firstCheck = 1;
                    gameStage++;
                }
                gameReady = true;
                if (aiFolds)
                {
                    CloseBettingAction();
                    PlayerWon(potSize);
                }
                else
                {
                    if (gameStage == 1 && !bigPause)
                    {
                        gameStage++;
                        StartCoroutine(DealFlop());
                        doneDealing = false;
                    }
                    if (gameStage == 2 && doneDealing && !bigPause)
                    {
                        gameStage++;
                        StartCoroutine(DealTurn());
                        doneDealing = false;
                    }
                    if (gameStage == 3 && doneDealing && !bigPause)
                    {
                        gameStage++;
                        StartCoroutine(DealRiver());
                        doneDealing = false;
                    }
                    if (gameStage > 3 && riverDealt && doneDealing)
                    {
                        StartCoroutine(WinningPause());
                        doneDealing = false;
                    }
                }
                playerMadeBet = false;
            }          
        }
        if (AiAllIn && !playerBusted && !aiBusted)
        {
            StartBettingAction();
            if (gameStage == 1 && !bigPause)
            {
                gameStage++;
                StartCoroutine(DealFlop());
                doneDealing = false;
            }
            if (gameStage == 2 && doneDealing && !bigPause)
            {
                gameStage++;
                StartCoroutine(DealTurn());
                doneDealing = false;
            }
            if (gameStage == 3 && doneDealing && !bigPause)
            {
                gameStage++;
                StartCoroutine(DealRiver());
                doneDealing = false;
            }
            if (gameStage > 3 && riverDealt && doneDealing)
            {
                StartCoroutine(WinningPause());
                doneDealing = false;
            }
        }

        

        if (playerFolds)
        {
            CloseBettingAction();
            AiWon(potSize);
            
        }
        else if (aiFolds)
        {
            CloseBettingAction();
            PlayerWon(potSize);
            ShowDealerFold();
            
        }

        //Move to late update if testing fails
        if (gameStage == 1 && aiMadeBet && aiBetReady && !aiFolds && !playerFolds && !playerBusted && !aiBusted && !resetting && comCardManager.cardSlot[0].cardData == null && !bigPause && !playerAllIn && !AiAllIn)
        {
            StartCoroutine(DealFlop());
            doneDealing = false;
        }
        else if (gameStage == 2 && aiMadeBet && aiBetReady && !aiFolds && !playerFolds && !playerBusted && !aiBusted && !resetting && comCardManager.cardSlot[3].cardData == null && !bigPause && doneDealing && !playerAllIn && !AiAllIn)
        {
            StartCoroutine(DealTurn());
            doneDealing = false;
        }
        else if (gameStage == 3 && aiMadeBet && aiBetReady && !aiFolds && !playerFolds && !playerBusted && !aiBusted && !resetting && comCardManager.cardSlot[4].cardData == null && !bigPause && doneDealing && !playerAllIn && !AiAllIn)
        {
            StartCoroutine(DealRiver());
            doneDealing = false;
        }

        if (gameStage > 3 && riverDealt && aiMadeBet && !gotWinner && doneDealing && !playerAllIn && !AiAllIn)
        {
            bigPause = true;
            StartCoroutine(WinningPause());
        }
        #region
        //Triggers for troubleshooting
        if (dealCard)
            CardDealing();
        if (resetHoleCards)
            ResetHoleCards();
        if (resetCardsStatus)
            ResetCardStatus();
        if (dealFlop)
            DealFlop();
        if (dealTurn)
            DealTurn();
        if (dealRiver)
            DealRiver();
        if (resetComCardSlots)
            ResetComCards();
        #endregion
    }

    private void LateUpdate()
    {
        
    }

    private void MassReset()
    {
        ResetTriggers();
        ResetHoleCards();
        ResetComCards();
        ResetCardStatus();
    }
    private void StartBettingAction()
    {
        uiManager.openPlayBetting();
        aiMadeBet = false;
        playerManager.triggerGetHandValue = true;
        
    }

    private void CloseBettingAction()
    {
        uiManager.closePlayBetting();
        playerManager.triggerGetHandValue = false;
    }
    public void CardDealing()
    {
        if (!bigPause && !stopDealing)
        {
            PlayingCard card = deck[Random.Range(0, deck.Length - 1)];
            bool cardHasBeenDeal = false;
            if (card.dealtThisHand)
            {
                cardHasBeenDeal = true;
            }
            while (cardHasBeenDeal)
            {
                card = deck[Random.Range(0, deck.Length - 1)];
                if (!card.dealtThisHand)
                {
                    cardHasBeenDeal = false;
                    break;
                }
            }

            if (playerDealSwitch == 0)
            {
                playerManager.GetDealt(card);
                playerDealSwitch++;
            }
            else if (playerDealSwitch == 1)
            {
                dealerManager.GetDealt(card);
                playerDealSwitch--;
            }

            card.dealtThisHand = true;
            dealCard = false;            
            aiBetReady = true;
            gameReady = false;
        }
    }

    private void PlayerWon(int potSizeIn)
    {
        playerStack.GetWinning(potSizeIn);
        Debug.Log("Player won" + potSize);
        uiManager.ShowPlayerWin();
        dealerManager.ShowCards();
        ResetTriggers();
        StartCoroutine(WaitOnStart());
    }

    private void AiWon(int potSizeIn)
    {
        dealerStack.GetWinning(potSizeIn);
        dealerManager.aiWon = true;
        uiManager.ShowDealerWin();
        dealerManager.ShowCards();
        Debug.Log("Ai Won" + potSize);
        ResetTriggers();
        StartCoroutine(WaitOnStart());
    }

    private void SplitAction(int potSizeIn)
    {
        dealerStack.GetWinning(potSizeIn/2);
        playerStack.GetWinning(potSizeIn/2);
        uiManager.ShowSplit();
        Debug.Log("Split Pot");
        ResetTriggers();
        MassReset();
        StartCoroutine(WaitOnStart());
    }

    private void PlayerBust()
    {
        stopDealing = true;
        uiManager.PlayerBust();
        ResetTriggers();
        StartCoroutine(WaitToReset());
    }
    private void DealerBust()
    {
        stopDealing = true;
        uiManager.DealerBust();
        StartCoroutine(WaitToReset());
    }
    private void GetWinner()
    {
        if (lastCheck == 0)
        {
            lastCheck = 1;
            gotWinner = true;
            playerManager.ReturnHandValueHighCard();
            playerOneHandValue = playerManager.handValue;
            playerOneHighCard = playerManager.currentHighCard;
            dealerManager.ReturnAiHandValueHighCard();
            playerTwoHandValue = dealerManager.handValue;
            playerTwoHighCard = dealerManager.currentHighCard;

            if (playerOneHandValue > playerTwoHandValue)
            {
                PlayerWon(potSize);
            }
            else if (playerOneHandValue < playerTwoHandValue)
            {
                AiWon(potSize);
            }
            else if (playerOneHandValue == playerTwoHandValue)
            {
                if (playerOneHighCard == playerTwoHighCard)
                {
                    SplitAction(potSize);
                }
                else if (playerOneHighCard > playerTwoHighCard || playerOneHighCard == 1)
                {
                    PlayerWon(potSize);
                }
                else if (playerOneHighCard < playerTwoHighCard || playerTwoHighCard == 1)
                {
                    AiWon(potSize);
                }
            }
        }
    }

    private void ShowDealerFold()
    {
        uiManager.ShowDealerFolds();

    }
    IEnumerator StartGame()
    {
        if (!stopDealing)
        {
            for (int i = 0; i < 4; i++)
            {
                yield return new WaitForSeconds(1);
                CardDealing();
            }
            doneDealing = true;
            playerBetReady = true;
        }
    }
    IEnumerator DealFlop()
    {
        aiMadeBet = false;
        playerMadeBet = false;
        for (int i = 0; i <= 2; i++)
        {
            PlayingCard card = deck[Random.Range(0, deck.Length)];

            while (card.dealtThisHand)
            {
                card = deck[Random.Range(0, deck.Length)];
            }
            // Debug.Log("Dealing Flop Card" + i);

            comCardManager.GetDealtFlop(card);
            yield return new WaitForSeconds(2);
            
        }
        dealFlop = false;
        doneDealing = true;
        playerBetReady = true;
        aiBetReady = true;
        gameReady=false;
    }
    IEnumerator DealTurn()
    {
        aiMadeBet = false;
        playerMadeBet = false;
        PlayingCard card = deck[Random.Range(0, deck.Length)];

        while (card.dealtThisHand)
        {
            card = deck[Random.Range(0, deck.Length)];
        }

        yield return new WaitForSeconds(1);
        comCardManager.GetDealtTurn(card);
        card.dealtThisHand = true;
        dealTurn = false;
        playerBetReady = true;
        aiBetReady = true;
        doneDealing=true;
        gameReady = false;
    }
    IEnumerator DealRiver()
    {
        aiMadeBet = false;
        playerMadeBet = false;
        PlayingCard card = deck[Random.Range(0, deck.Length)];

        while (card.dealtThisHand)
        {
            card = deck[Random.Range(0, deck.Length)];
        }

        yield return new WaitForSeconds(1);
        comCardManager.GetDealtRiver(card);
        card.dealtThisHand = true;
        dealRiver = false;
        doneDealing = true;
        playerBetReady = true;
        aiBetReady = true;
        gameReady = false;
        riverDealt = true;
    }

    private void ResetHoleCards()
    {
       // Debug.Log("Reseting hole cards");
        playerManager.ResetHand();
        dealerManager.ResetHand();
        resetHoleCards = false;
        playerManager.ClearHandValues();
        dealerManager.ClearHandValues();
    }
    private void ResetComCards()
    {
        //Debug.Log("Reseting Com Cards");
        comCardManager.ResetHand();
        resetComCardSlots = false;
    }

    private void ResetCardStatus()
    {
        resetCardsStatus = false;
        for (int i = 0; i < deck.Length; i++)
        {
            if (deck[i] != null)
                 deck[i].dealtThisHand = false;
        }       
    }

    private void ResetTriggers()
    {
        aiBetReady = false;
        playerBetReady = false;
        aiMadeBet = false;
        playerMadeBet = false;
        doneDealing = false;


        playerAllIn = false;
        playerFolds = false;
        playerCall = false;
        gotWinner = false;

        AiAllIn = false;
        aiFolds = false;
        aiCall = false;
        aiRaise = false;
        playerLastBet = 0;

        gameReady = false;
        riverDealt = false;
        
        playerDealSwitch = 0;
        gameStage = 0;
        firstCheck = 0;

        playerOneHandValue = 0;
        playerTwoHandValue = 0;
        playerOneHighCard = 0;
        playerTwoHighCard = 0;
        potSize = 0;
        potCounter.ResetPot();
        playerManager.triggerGetHandValue = false;
        dealerManager.triggerGetHandValue = false;
    }
    IEnumerator WaitOnStart()
    {      
        yield return new WaitForSeconds(5);
        MassReset();
        comCardManager.ResetHand();
        if (!resetting)
        {
            lastCheck = 0;
            StartCoroutine(StartGame());
        }
    }
    IEnumerator WaitToReset()
    {
        resetting = true;
        ResetTriggers();
        yield return new WaitForSeconds(5);
        stopDealing = false;
        lastCheck = 0;
        MassReset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator WinningPause()
    {
        yield return new WaitForSeconds(5);
        GetWinner();
        bigPause = false;
    }
}
