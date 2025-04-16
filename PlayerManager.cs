using System.Collections;
using System;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class PlayerManager : MonoBehaviour
{
    [Header("Components")]
    public CardSlotManager[] cardSlot;
    public CardSlotManager[] comCards;
    public CardValueSlider cardValueSlider;

    [Header("Gambling Info")]
    public int stack = 1000;
    public int lastBet;
    public int handValue;
    public int currentHighCard;

    [Header("Triggers")]
    public bool fullHand;
    public bool resetHand;
    public bool triggerGetHandValue;

    [Header("Arrays")]
    public int[] posStraightCards = new int[7];

    [Header("Hand Value Bools")]
    public int highCard;
    public bool onePair;
    public bool twoPair;
    public bool threeOfKind;
    public bool straight;
    public bool flush;
    public bool fullHouse;
    public bool fourOfKind;
    public bool straightFlush;
    public bool royalFlush;

    //board pairs
    public int bHighCard;
    public bool bOnePair;
    public bool bTwoPair;
    public bool bThreeOfKind;
    public bool bStraight;
    public bool bFlush;
    public bool bFullHouse;
    public bool bFourOfKind;
    public bool bStraightFlush;
    public bool bRoyalFlush;
    public bool bNone;

    private void Update()
    {
        
        if (cardSlot[0].isFull && cardSlot[1].isFull)
        {
            fullHand = true;
        }

        if(resetHand) 
            ResetHand();
        if (triggerGetHandValue)
        {
            ReturnHandValueHighCard();
            triggerGetHandValue = false;
        }
    }

    public void GetDealt(PlayingCard newCard)
    {
        Sprite newCardSprite = newCard.cardFace;

        if (!fullHand)
        {
            if (!cardSlot[0].isFull)
            {
                cardSlot[0].ChangeSlot(newCardSprite, newCard);
               // return;
            }
            else if (!cardSlot[1].isFull)
            {
                cardSlot[1].ChangeSlot(newCardSprite, newCard);
               // return;
            }
        }
        else
        {
           // return;
        }
       
    }

    public void ResetHand()
    {
        for (int i = 0; i < cardSlot.Length; i++)
        {
            cardSlot[i].ClearSlot();
                fullHand = false;
            cardSlot[i].newCardSprite = null;
        }
        
        resetHand = false;
        
    }
    public void GetHandValue()
    {
        triggerGetHandValue = false;
        PlayingCard cardDataOne = cardSlot[0].cardData;
        PlayingCard cardDataTwo = cardSlot[1].cardData;

        
        
        CheckForPairs();

        if (comCards[3].cardData != null)
        {
            CheckForStraight();
            CheckForFlush();
            CheckForFullHouse();
            CheckForRoyalFlush();
        }
         
        if(highCard == 0)
          GetHoleHighCard();
        
    }
    public void CheckForFlush()
    {
        PlayingCard[] collectiveCards = new PlayingCard[cardSlot.Length + comCards.Length];
        collectiveCards[0] = cardSlot[0].cardData;
        collectiveCards[1] = cardSlot[1].cardData;

        if (comCards[3].cardData == null)
            return;

        int spadeCount = 0;
        int clubCount = 0;
        int heartCount = 0;
        int diamondCount = 0;

        int bSpadeCount = 0;
        int bClubCount = 0;
        int bHeartCount = 0;
        int bDiamondCount = 0;

        for (int i = 0; i < comCards.Length; i++)
        {
            if (comCards[i].cardData != null)
            {
                switch (comCards[i].cardData.cardSuit)
                {
                    case PlayingCard.Suit.Spade:
                        bSpadeCount++;
                        break;
                    case PlayingCard.Suit.Club:
                        bClubCount++;
                        break;
                    case PlayingCard.Suit.Heart:
                        bHeartCount++;
                        break;
                    case PlayingCard.Suit.Diamond:
                        bDiamondCount++;
                        break;
                }
            }
        }
        if (bSpadeCount == 5 || bClubCount == 5 || bHeartCount == 5 || bDiamondCount == 5)
        {
            bFlush = true;
            if (bSpadeCount == 5)
            {
                if (cardSlot[0].cardData.cardValue > cardSlot[1].cardData.cardValue && cardSlot[0].cardData.cardSuit == PlayingCard.Suit.Spade)
                {
                    flush = true;
                    highCard = cardSlot[0].cardData.cardValue;
                }
                else if (cardSlot[1].cardData.cardValue > cardSlot[0].cardData.cardValue && cardSlot[1].cardData.cardSuit == PlayingCard.Suit.Spade)
                {
                    flush = true;
                    highCard = cardSlot[1].cardData.cardValue;
                }
            }
            else if (bClubCount == 5)
            {
                if (cardSlot[0].cardData.cardValue > cardSlot[1].cardData.cardValue && cardSlot[0].cardData.cardSuit == PlayingCard.Suit.Club)
                {
                    flush = true;
                    highCard = cardSlot[0].cardData.cardValue;
                }
                else if (cardSlot[1].cardData.cardValue > cardSlot[0].cardData.cardValue && cardSlot[1].cardData.cardSuit == PlayingCard.Suit.Club)
                {
                    flush = true;
                    highCard = cardSlot[1].cardData.cardValue;
                }
            }
            else if (bHeartCount == 5)
            {
                if (cardSlot[0].cardData.cardValue > cardSlot[1].cardData.cardValue && cardSlot[0].cardData.cardSuit == PlayingCard.Suit.Heart)
                {
                    flush = true;
                    highCard = cardSlot[0].cardData.cardValue;
                }
                else if (cardSlot[1].cardData.cardValue > cardSlot[0].cardData.cardValue && cardSlot[1].cardData.cardSuit == PlayingCard.Suit.Heart)
                {
                    flush = true;
                    highCard = cardSlot[1].cardData.cardValue;
                }
            }
            else if (bDiamondCount == 5)
            {
                if (cardSlot[0].cardData.cardValue > cardSlot[1].cardData.cardValue && cardSlot[0].cardData.cardSuit == PlayingCard.Suit.Diamond)
                {
                    flush = true;
                    highCard = cardSlot[0].cardData.cardValue;
                }
                else if (cardSlot[1].cardData.cardValue > cardSlot[0].cardData.cardValue && cardSlot[1].cardData.cardSuit == PlayingCard.Suit.Diamond)
                {
                    flush = true;
                    highCard = cardSlot[1].cardData.cardValue;
                }
            }
        }

        for (int i = 0; i < comCards.Length; i++)
        {
            collectiveCards[i + 2] = comCards[i].cardData;
        }

        for (int i = 0; i < collectiveCards.Length; i++)
        {
            if (collectiveCards[i] != null)
            {
                switch (collectiveCards[i].cardSuit)
                {
                    case PlayingCard.Suit.Spade:
                        spadeCount++;
                        break;
                    case PlayingCard.Suit.Club:
                        clubCount++;
                        break;
                    case PlayingCard.Suit.Heart:
                        heartCount++;
                        break;
                    case PlayingCard.Suit.Diamond:
                        diamondCount++;
                        break;
                }
            }
        }
       // Debug.Log(spadeCount + "+" + clubCount + "+" + heartCount + "+" + diamondCount);
        if (spadeCount >= 5 || clubCount >= 5 || heartCount >= 5 || diamondCount >= 5)
        {      
            if (spadeCount >= 5)
            {
                if (cardSlot[0].cardData.cardSuit == PlayingCard.Suit.Spade)
                {
                    flush = true;
                    highCard = cardSlot[0].cardData.cardValue;
                }
                else if (cardSlot[1].cardData.cardSuit == PlayingCard.Suit.Spade)
                {
                    flush = true;
                    highCard = cardSlot[1].cardData.cardValue;
                }
            }
            else if (clubCount >= 5)
            {
               
                if (cardSlot[0].cardData.cardSuit == PlayingCard.Suit.Club)
                {
                    flush = true;
                    highCard = cardSlot[0].cardData.cardValue;
                }
                else if (cardSlot[1].cardData.cardSuit == PlayingCard.Suit.Club)
                {
                    flush = true;
                    highCard = cardSlot[1].cardData.cardValue;
                }
            }
            else if (heartCount >= 5)
            {
                if (cardSlot[0].cardData.cardSuit == PlayingCard.Suit.Heart)
                {
                    flush = true;
                    highCard = cardSlot[0].cardData.cardValue;
                }
                else if (cardSlot[1].cardData.cardSuit == PlayingCard.Suit.Heart)
                {
                    flush = true;
                    highCard = cardSlot[1].cardData.cardValue;
                }
            }
            else if (diamondCount >= 5)
            {
                if (cardSlot[0].cardData.cardSuit == PlayingCard.Suit.Diamond)                
                {
                    flush = true;
                    highCard = cardSlot[0].cardData.cardValue;
                }
                else if (cardSlot[1].cardData.cardSuit == PlayingCard.Suit.Diamond)
                {
                    flush = true;
                    highCard = cardSlot[1].cardData.cardValue;
                }
            }
        }
    }
 
    public void CheckForPairs()
    {
        int cardOneMatches = 0;
        int cardTwoMatches = 0;

        int[] comMatches = new int[comCards.Length];
        int twoPairCheck = 0;

        if (comCards[2].cardData != null)
        {
            //Check for com card matches
            for (int i = 0; i < comCards.Length; i++)
            {
                for (int j = 0; j < comCards.Length; j++)
                {
                    if (comCards[i].cardData != null && comCards[j].cardData != null)
                    {
                        if (comCards[i].cardData.cardValue == comCards[j].cardData.cardValue && i != j)
                            comMatches[i]++;
                    }
                }
            }


            //Report Com Card Hands
            for (int i = 0; i < comMatches.Length; i++)
            {
                if (comMatches[i] == 1)
                {
                    bOnePair = true;
                    twoPairCheck++;
                }
                if (comMatches[i] == 2)
                {
                    bThreeOfKind = true;
                }
                if (comMatches[i] == 3)
                {
                    bFourOfKind = true;
                }
            }
            if (twoPairCheck >= 3)
            {
                bTwoPair = true;
            }
        }
        //for(int i = 0; i < comMatches.Length; i++)
        //{         
        //    Debug.Log("ComMatches " + i + " = " + comMatches[i]);
        //}


        #region
        if (comCards[0].cardData != null)
        {
            //Check for hole card matches
            for (int i = 0; i < comCards.Length; i++)
            {
                if (comCards[i].cardData == null || comCards[0].cardData == null || cardSlot[1].cardData == null)
                {
                    break;
                }
                else if (comCards[i].cardData != null)
                {
                    if (comCards[i].cardData.cardValue == cardSlot[0].cardData.cardValue)
                    {
                        cardOneMatches++;
                    }
                    if (comCards[i].cardData.cardValue == cardSlot[1].cardData.cardValue)
                    {
                        cardTwoMatches++;
                    }
                    
                }
            }
        }

        bool holePair = false;
        if (cardSlot[0].cardData == null || cardSlot[1].cardData == null)
        {
            return;
        }
        else
        {
            if (cardSlot[0].cardData.cardValue == cardSlot[1].cardData.cardValue)
            {
                onePair = true;
                holePair = true;
                cardOneMatches++;
                cardTwoMatches++;
            }
        }
        //Report Hole Card Hands
        
        if ((cardOneMatches == 1 && cardTwoMatches == 0))
        {
            highCard = cardSlot[0].cardData.cardValue; 
            onePair = true;
        }
        if ((cardOneMatches == 0 && cardTwoMatches == 1))
        {
            onePair = true;
            highCard = cardSlot[1].cardData.cardValue;
        }
        if (cardOneMatches == 1 && cardTwoMatches == 1 )
        {
            if (!holePair)
            {
                twoPair = true;
                GetHoleHighCard();
            }
        }
        if (cardOneMatches == 2 ||  cardTwoMatches == 2)
        {
            threeOfKind = true;
            if (cardOneMatches == 2)
                highCard = cardSlot[0].cardData.cardValue;
            else if (cardTwoMatches == 2)
                highCard = cardSlot[1].cardData.cardValue;
            else
                GetHoleHighCard();
        }
        if (cardOneMatches == 3 || cardTwoMatches == 3)
        {
            fourOfKind = true;
            if (cardOneMatches == 3)
                highCard = cardSlot[0].cardData.cardValue;
            else if (cardTwoMatches == 3)
                highCard = cardSlot[1].cardData.cardValue;
            else
                GetHoleHighCard();
        }
        #endregion
    }

    public void CheckForStraight()
    {
        PlayingCard[] collectiveCard = new PlayingCard[cardSlot.Length + comCards.Length];

        int[] comCardValues = new int[comCards.Length];
        posStraightCards[0] = cardSlot[0].cardData.cardValue;
        posStraightCards[1] = cardSlot[1].cardData.cardValue;

        collectiveCard[0] = cardSlot[0].cardData;
        collectiveCard[1] = cardSlot[1].cardData;

        if (comCards[3].cardData == null)
            return;
        
        for (int i = 0; i < comCards.Length; i++)
        {
            if (comCards[i].cardData != null)
            {
                comCardValues[i] = comCards[i].cardData.cardValue;
                posStraightCards[i + 2] = comCards[i].cardData.cardValue;
                collectiveCard[i + 2] = comCards[i].cardData;
            }
        }

        Array.Sort(comCardValues);

        if (comCardValues[4] != 0)
        {
            if (comCardValues[1] - comCardValues[0] == 1 && comCardValues[2] - comCardValues[1] == 1 && comCardValues[3] - comCardValues[2] == 1 && comCardValues[4] - comCardValues[3] == 1)
            {
                bHighCard = comCardValues[4];
                bStraight = true;
                bStraightFlush = true;
                for (int i = 0; i < comCards.Length; i++)
                {
                    if (comCards[0].cardData.cardSuit != comCards[i].cardData.cardSuit)
                    {
                        bStraightFlush = false;
                    }
                }
            }
        }

        Array.Sort(posStraightCards);
        straight = false;
        int cardFailures = 0;
        int straightCardCounter = 0;
        bool midFailure = false;

        
        for (int i = 0; i < posStraightCards.Length; i++) 
        {
            if (posStraightCards[i] == 0)
            {
                cardFailures++;
               // Debug.Log("Card Failure" + cardFailures);
            }
            else if (i != posStraightCards.Length - 1 && straightCardCounter < 4)
            {
                if (posStraightCards[i + 1] - posStraightCards[i] == 1 && cardFailures < 3)
                {
                    straightCardCounter++;
                    //Debug.Log("Straight counter" + straightCardCounter);
                }
                else if (posStraightCards[i + 1] - posStraightCards[i] != 1 && i > 2)
                {
                    midFailure = true;
                    cardFailures = 4;
                }              
                else
                {
                    cardFailures++;
                   // Debug.Log("Card Failure" + cardFailures);
                }
            }
            else if(i == 6 && straightCardCounter < 4)
            {
                
                if (posStraightCards[6] - posStraightCards[5] == 1 && cardFailures < 3)
                {
                    straightCardCounter++;
                   // Debug.Log("Straight counter" + straightCardCounter);
                }
                else
                {
                    cardFailures++;
                }
            }
            if (straightCardCounter >= 4)
            {
                straight = true;
                bHighCard = posStraightCards[i];              
            }
            else if (i == posStraightCards.Length)
            {
                break;
            }      
        }

        if (straightCardCounter >= 4 && !midFailure)
        {
            straight = true;           
        }
        else
        {
            straight = false;
        }
     
        int spadeCount = 0;
        int clubCount = 0;
        int heartCount = 0;
        int diamondCount = 0;

        if (straight)
        {
            for (int i = 0; i < collectiveCard.Length; i++) 
            {
                if (collectiveCard[i] != null)
                {
                    switch (collectiveCard[i].cardSuit)
                    {
                        case PlayingCard.Suit.Spade:
                            spadeCount++;
                            break;
                        case PlayingCard.Suit.Club:
                            clubCount++;
                            break;
                        case PlayingCard.Suit.Heart:
                            heartCount++;
                            break;
                        case PlayingCard.Suit.Diamond:
                            diamondCount++;
                            break;
                    }
                }
            }
        }

        if (spadeCount == 5 || clubCount == 5 || heartCount == 5 || diamondCount == 5)
        {
            straightFlush = true;
        }  
        
        CheckForBroadway();
        CheckForWheel();
        CheckForWheelStraightFlush();
    }

    public void CheckForFullHouse()
    {
        if (onePair && bThreeOfKind)
        {
            fullHouse = true;
        }
        else if (threeOfKind && bOnePair)
        {
            fullHouse = true;
        }
        else if (bTwoPair && bThreeOfKind)
        {
            fullHouse = true;
        }       
    }
    public void CheckForRoyalFlush()
    {
        PlayingCard[] collectiveCard = new PlayingCard[cardSlot.Length + comCards.Length];
        PlayingCard.Suit[] suits = new PlayingCard.Suit[4] {PlayingCard.Suit.Spade, PlayingCard.Suit.Club, PlayingCard.Suit.Heart, PlayingCard.Suit.Diamond};

        collectiveCard[0] = cardSlot[0].cardData;
        collectiveCard[1] = cardSlot[1].cardData;

        int[,] posRoyalFlush = { { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 } };
        
        
        #region
        //bool aceOfS;
        //bool aceOfC;
        //bool aceOfH;
        //bool aceOfD;

        //bool kOfS;
        //bool kOfC;
        //bool kOfH;
        //bool kOfD;

        //bool qOfS;
        //bool qOfC;
        //bool qOfH;
        //bool qOfD;

        //bool jOfS;
        //bool jOfC;
        //bool jOfH;
        //bool jOfD;

        //bool tenOfS;
        //bool tenOfC;
        //bool tenOfH;
        //bool tenOfD;
        #endregion

        for (int i = 0; i < comCards.Length; i++)
        {
            collectiveCard[i + cardSlot.Length] = comCards[i].cardData;
        }

        for (int i = 0; i < collectiveCard.Length; i++)
        {
            for (int j = 0; j < suits.Length; j++)
            {
                if (collectiveCard[i] != null)
                {
                    if (collectiveCard[i].cardSuit == suits[j])
                    {
                        if (collectiveCard[i].cardValue == 1)
                            posRoyalFlush[j, 0] = 1;
                        else if (collectiveCard[i].cardValue == 10)
                            posRoyalFlush[j, 1] = 1;
                        else if (collectiveCard[i].cardValue == 11)
                            posRoyalFlush[j, 2] = 1;
                        else if (collectiveCard[i].cardValue == 12)
                            posRoyalFlush[j, 3] = 1;
                        else if (collectiveCard[i].cardValue == 13)
                            posRoyalFlush[j, 4] = 1;
                    }
                }
            }
        }

        

        bool[] royalFlushes = new bool[4] {true, true, true, true};
        

        for (int i = 0; i < posRoyalFlush.GetLength(0); i++)
        {
            for (int j = 0; j < posRoyalFlush.GetLength(1); j++)
            {
                //Debug.Log("Position " + i + " , " + j + "'" + posRoyalFlush[i, j]);
                if (posRoyalFlush[i, j] != 1)
                    royalFlushes[i] = false;
            }
        }

        for (int i = 0; i < suits.Length; i++)
        {
            if (royalFlushes[i] && collectiveCard[0].cardSuit == suits[i])
            {
                royalFlush = true;
                highCard = collectiveCard[0].cardValue;
            }
            else if (royalFlushes[i] && collectiveCard[1].cardSuit == suits[i])
            {
                royalFlush = true;
                if (collectiveCard[1].cardValue > highCard)
                {
                    highCard = collectiveCard[1].cardValue;
                }
            }
            else
            {
                //royalFlush = false;
            }
        }

        if (royalFlushes[0] || royalFlushes[1] || royalFlushes[2] || royalFlushes[3])
            bRoyalFlush = true;
    }

    public void CheckForWheelStraightFlush()
    {
        PlayingCard[] collectiveCard = new PlayingCard[cardSlot.Length + comCards.Length];
        PlayingCard.Suit[] suits = new PlayingCard.Suit[4] { PlayingCard.Suit.Spade, PlayingCard.Suit.Club, PlayingCard.Suit.Heart, PlayingCard.Suit.Diamond };

        collectiveCard[0] = cardSlot[0].cardData;
        collectiveCard[1] = cardSlot[1].cardData;

        int[,] posRoyalFlush = { { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 } };

        for (int i = 0; i < comCards.Length; i++)
        {
            collectiveCard[i + cardSlot.Length] = comCards[i].cardData;
        }

        for (int i = 0; i < collectiveCard.Length; i++)
        {
            for (int j = 0; j < suits.Length; j++)
            {
                if (collectiveCard[i] != null)
                {
                    if (collectiveCard[i].cardSuit == suits[j])
                    {
                        if (collectiveCard[i].cardValue == 1)
                            posRoyalFlush[j, 0] = 1;
                        else if (collectiveCard[i].cardValue == 2)
                            posRoyalFlush[j, 1] = 1;
                        else if (collectiveCard[i].cardValue == 3)
                            posRoyalFlush[j, 2] = 1;
                        else if (collectiveCard[i].cardValue == 4)
                            posRoyalFlush[j, 3] = 1;
                        else if (collectiveCard[i].cardValue == 5)
                            posRoyalFlush[j, 4] = 1;
                    }
                }
            }
        }

        bool[] royalFlushes = new bool[4] { true, true, true, true };


        for (int i = 0; i < posRoyalFlush.GetLength(0); i++)
        {
            for (int j = 0; j < posRoyalFlush.GetLength(1); j++)
            {
                //Debug.Log("Position " + i + " , " + j + "'" + posRoyalFlush[i, j]);
                if (posRoyalFlush[i, j] != 1)
                    royalFlushes[i] = false;
            }
        }

        for (int i = 0; i < suits.Length; i++)
        {
            if (royalFlushes[i] && collectiveCard[0].cardSuit == suits[i])
            {
                straightFlush = true;
                highCard = collectiveCard[0].cardValue;
            }
            else if (royalFlushes[i] && collectiveCard[1].cardSuit == suits[i])
            {
                straightFlush = true;
                if (collectiveCard[1].cardValue > highCard)
                {
                    highCard = collectiveCard[1].cardValue;
                }
            }
            else
            {
                //royalFlush = false;
            }
        }

        if (royalFlushes[0] || royalFlushes[1] || royalFlushes[2] || royalFlushes[3])
            bStraightFlush = true;
    }
    public void CheckForBroadway()
    {
        PlayingCard[] collectiveCard = new PlayingCard[cardSlot.Length + comCards.Length];
        PlayingCard.Suit[] suits = new PlayingCard.Suit[4] { PlayingCard.Suit.Spade, PlayingCard.Suit.Club, PlayingCard.Suit.Heart, PlayingCard.Suit.Diamond };

        collectiveCard[0] = cardSlot[0].cardData;
        collectiveCard[1] = cardSlot[1].cardData;

        int[] posBroadway = { 0, 0, 0, 0, 0 };
        int[] necValues = { 1, 10, 11, 12, 13 };

        bool foundBroadway;

        for (int i = 0; i < comCards.Length; i++)
        {
            collectiveCard[i + cardSlot.Length] = comCards[i].cardData;
        }

        for (int i = 0; i < collectiveCard.Length; i++)
        {
            if (collectiveCard[i].cardValue == 1)
                posBroadway[0] = 1;
            else if (collectiveCard[i].cardValue == 10)
                posBroadway[1] = 1;
            else if (collectiveCard[i].cardValue == 11)
                posBroadway[2] = 1;
            else if (collectiveCard[i].cardValue == 12)
                posBroadway[3] = 1;
            else if (collectiveCard[i].cardValue == 13)
                posBroadway[4] = 1;
            else
                break;
        }

        int startingCount = posBroadway[0];

        for (int i = 1; i < posBroadway.Length; i++)
        {
            startingCount += posBroadway[i];
        }

        if (startingCount >= 5)
            foundBroadway = true;
        else 
            foundBroadway = false;

        for (int i = 0; i < necValues.Length; i++) 
        { 
            if (foundBroadway && collectiveCard[0].cardValue == necValues[i])
            {
                straight = true;
                highCard = collectiveCard[0].cardValue;
            }
            if (foundBroadway && collectiveCard[1].cardValue == necValues[i])
            {
                straight = true;
                if (collectiveCard[1].cardValue > highCard)
                {
                    highCard = collectiveCard[1].cardValue;
                }
            }
        }       
    }
    public void CheckForWheel()
    {
        PlayingCard[] collectiveCard = new PlayingCard[cardSlot.Length + comCards.Length];
        PlayingCard.Suit[] suits = new PlayingCard.Suit[4] { PlayingCard.Suit.Spade, PlayingCard.Suit.Club, PlayingCard.Suit.Heart, PlayingCard.Suit.Diamond };

        collectiveCard[0] = cardSlot[0].cardData;
        collectiveCard[1] = cardSlot[1].cardData;

        int[] posWheel = { 0, 0, 0, 0, 0 };
        int[] necValues = { 1, 2, 3, 4, 5 };

        bool foundWheel;

        for (int i = 0; i < comCards.Length; i++)
        {
            collectiveCard[i + cardSlot.Length] = comCards[i].cardData;
        }

        int x = 0;

        for (int i = 0; i < collectiveCard.Length; i ++)
        {
            if (collectiveCard[i] == null)
                x = i;
        }

        for (int i = 0; i < x; i++)
        {
            //Debug.Log(collectiveCard[i]);
            if (collectiveCard[i] == null)
                break;
            else if (collectiveCard[i].cardValue == 1)
                posWheel[0] = 1;
            else if (collectiveCard[i].cardValue == 10)
                posWheel[1] = 1;
            else if (collectiveCard[i].cardValue == 11)
                posWheel[2] = 1;
            else if (collectiveCard[i].cardValue == 12)
                posWheel[3] = 1;
            else if (collectiveCard[i].cardValue == 13)
                posWheel[4] = 1;
            else
                break;
            
        }

        int startingCount = posWheel[0];

        for (int i = 1; i < posWheel.Length; i++)
        {
            startingCount += posWheel[i];
        }

        if (startingCount >= 5)
            foundWheel = true;
        else
            foundWheel = false;

        for (int i = 0; i < necValues.Length; i++)
        {
            if (foundWheel && collectiveCard[0].cardValue == necValues[i])
            {
                straight = true;
                highCard = collectiveCard[0].cardValue;
            }
            if (foundWheel && collectiveCard[1].cardValue == necValues[i])
            {
                straight = true;
                if (collectiveCard[1].cardValue > highCard)
                {
                    highCard = collectiveCard[1].cardValue;
                }
            }
        }
    }
    public void GetHoleHighCard()
    {
        if (cardSlot[0].cardData == null || cardSlot[1].cardData == null)
        { return; }
        else
        {
            if (cardSlot[0].cardData.cardValue > cardSlot[1].cardData.cardValue || cardSlot[0].cardData.cardValue == 1)
                highCard = cardSlot[0].cardData.cardValue;
            else if (cardSlot[1].cardData.cardValue > cardSlot[0].cardData.cardValue || cardSlot[1].cardData.cardValue == 1)
                highCard = cardSlot[1].cardData.cardValue;
            else
                highCard = cardSlot[1].cardData.cardValue;
        }
    }

    public void ReturnHandValueHighCard()
    {
        if (cardSlot[0].cardData == null)
        { return; }
        else {
            //Debug.Log(highCard); 
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

            cardValueSlider.sliderValue = handValue;
        }
    }


    public void ClearHandValues()
    {
        highCard = 0;
        lastBet = 0;
        currentHighCard = 0;
        onePair = false;
        twoPair = false;
        threeOfKind = false;
        straight = false;
        flush = false;
        fullHouse = false;
        fourOfKind = false;
        straightFlush = false;
        royalFlush = false;

        //board pairs
        bHighCard = 0;
        bOnePair = false;
        bTwoPair = false;
        bThreeOfKind = false;
        bStraight = false;
        bFlush = false;
        bFullHouse = false;
        bFourOfKind = false;
        bStraightFlush = false;
        bRoyalFlush = false;
        bNone = false;
    }
}
