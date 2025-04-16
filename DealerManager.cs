using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class DealerManager : PlayerManager
{
    public StackCounter stackCounter;
    public BetCounter betCounter;
    public PotCounter potCounter;
    public GameManager gameManager;


    public int aiBet = 0;
    public int aiStack = 1000;
    public int aiLastBet;
    public bool cardsBeingDealt = false;
    public bool call = false;
    public bool fold = false;

    public bool resetShow = false;
    public bool aiWon = false;  

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();  
    }
    private void Update()
    {
        if (aiStack < 0)
        {
            aiStack = 0;    
        }
        stackCounter.stack = aiStack;
        if (stack < 5)
        {
            gameManager.AiAllIn = true;
        }
        else
        {
            gameManager.AiAllIn = false;
        }
        if (cardsBeingDealt)
        {
            StartCoroutine(ResetAiBet());
        }
        if (aiWon)
        {
            ShowCards();
        }
    }

    public void getAiBet(int minBet)
    {

        //Debug.Log("Ai betting" +  aiBet);
        GetAiHandValue();
       // Debug.Log(handValue);

        AiCall(minBet);
        AiFold(minBet);
        if (minBet < 10)
            fold = false;
        if (!gameManager.playerAllIn)
        {
            if (fold)
            {
                aiBet = 0;
                gameManager.aiFolds = true;
                aiStack = aiStack - aiBet;
                Debug.Log("Folding");

            }
            else if (call)
            {
                aiBet = minBet;
                aiStack = aiStack - aiBet;
                Debug.Log("call");
            }
            else
            {
                aiBet = (handValue * handValue) + minBet;

                Debug.Log("Considering raise");
                if (aiBet <= minBet)
                {
                    aiBet = minBet; //remove +1 after raise test
                    Debug.Log("Calling");
                    aiStack = aiStack - aiBet;
                }
                else if (stack < 5)
                {
                    aiBet = stack;
                    gameManager.AiAllIn = true;
                    aiStack = aiStack - aiBet;
                    Debug.Log("All In");
                }
                else
                {
                    aiStack = aiStack - aiBet;
                }
            }
        }
        else
        {
            if (!fold)
            {
                if (minBet < aiStack)
                {
                    aiBet = minBet;
                }
                else
                {
                    aiBet = aiStack;
                }

                aiStack = aiStack - aiBet;
            }
            else
            {
                gameManager.aiFolds = true;
            }
        }

        betCounter.bet = aiBet;
        potCounter.pot += aiBet;
        
        aiLastBet = aiBet;
        if (aiLastBet > gameManager.playerLastBet)
        {
            gameManager.aiRaise = true;
            StartCoroutine(ResetAiBet());
        }
        gameManager.playerTwoHandValue = handValue;
        gameManager.aiMadeBet = true;       
    }

    public void ShowCards()
    {
        resetShow = false;
        cardSlot[0].ShowCards();
        cardSlot[1].ShowCards();

        StartCoroutine(ResetShowCards());

        if (resetShow)
        {
            cardSlot[0].HideCards();
            cardSlot[1].HideCards();
        }
    }
    public void GetAiHandValue()
    {
        PlayingCard cardDataOne = cardSlot[0].cardData;
        PlayingCard cardDataTwo = cardSlot[1].cardData;


        triggerGetHandValue = false;
        CheckForPairs();

        if (comCards[3].cardData != null)
        {
            CheckForStraight();
            CheckForFlush();
            CheckForFullHouse();
            CheckForRoyalFlush();
        }
        ReturnAiHandValueHighCard();

        if (highCard == 0)
            GetHoleHighCard();
    }
    public void ReturnAiHandValueHighCard()
    {
        GetHandValue();
        if (royalFlush)
        {
            handValue = 10;
            currentHighCard = highCard;
            if (highCard == 0)
                GetHoleHighCard();
        }
        else if (straightFlush)
        {
            handValue = 9;
            currentHighCard = highCard;
            if (highCard == 0)
                GetHoleHighCard();
        }
        else if (fourOfKind)
        {
            handValue = 8;
            currentHighCard = highCard;
            if (highCard == 0)
                GetHoleHighCard();
        }
        else if (fullHouse)
        {
            handValue = 7;
            currentHighCard = highCard;
            if (highCard == 0)
                GetHoleHighCard();
        }
        else if (flush)
        {
            handValue = 6;
            currentHighCard = highCard;
            if (highCard == 0)
                GetHoleHighCard();
        }
        else if (straight)
        {
            handValue = 5;
            currentHighCard = highCard;
            if (highCard == 0)
                GetHoleHighCard();
        }
        else if (threeOfKind)
        {
            handValue = 4;
            currentHighCard = highCard;
            if (highCard == 0)
                GetHoleHighCard();
        }
        else if (twoPair)
        {
            handValue = 3;
            currentHighCard = highCard;
            if (highCard == 0)
                GetHoleHighCard();
        }
        else if (onePair)
        {
            handValue = 2;
            currentHighCard = highCard;
            if (highCard == 0)
                GetHoleHighCard();
        }
        else
        {
            handValue = 1;
            currentHighCard = highCard;
            if (highCard == 0)
            {
                GetHoleHighCard();
            }
        }
    }
    public void AiCall(int minBet)
    {
        if (handValue <= 4 && minBet <= aiStack/10)
        {
            call = true;

        }
        else if (handValue >= 4 && handValue <= 6 && minBet <= aiStack/5)
        {
            call = true;

        }
        else
        {
            call = false;
        }
    }
    public void AiFold(int minBet)
    {
        if (handValue <= 3 && minBet >= aiStack / 10)
        {
            int x = Random.Range(0, 10);
            if (x < 8)
            {
                fold = true;

            }
            else
            {
                fold = false;
            }
        }
        else
        {
            fold = false;
        }
    }
    public void ResetAiTriggers()
    {

    }
    public IEnumerator ResetAiBet()
    {
        yield return new WaitForSeconds(1);
        aiBet = 0;      
    }
    public IEnumerator ResetShowCards()
    {
        yield return new WaitForSeconds(2);
        resetShow = true;
        aiWon = false;
    }
}
