using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class CardSlotManager : MonoBehaviour
{
    public UnityEngine.UI.Image cardSlot;
    public Sprite defaultSprite;
    public Sprite newCardSprite;
    public PlayingCard cardData;
    public bool isFull;
    public bool isAiCardSlot;
    public bool showingCards;
    public bool alphaSwitch = false;

    public void Start()
    {
        cardSlot = GetComponent<UnityEngine.UI.Image>();   
    }

    public void Update()
    {
        
        if (cardData == null || cardSlot.sprite == null)
        {
            cardSlot.color = new Color(0f, 0f, 0f, 0f); //test
            cardSlot.enabled = false;

        }
        else
        {
            cardSlot.color = new Color(255f, 255f, 255f, 255f); //test
            cardSlot.enabled = true;    
        }
    }

    public void ChangeSlot(Sprite incomingCard, PlayingCard newCardData)
    {
        newCardSprite = incomingCard;   
        cardSlot.sprite = newCardSprite;
        cardData = newCardData; 
        cardSlot.color += new Color(0f, 0f, 0f, 1f);
        isFull = true;
        UpdateSprite();
    }
    public void UpdateSprite()
    {
        if (cardData != null)
        {
            if (!isAiCardSlot)
            {
                cardSlot.sprite = cardData.cardFace;
            }
            else if (isAiCardSlot && !showingCards)
            {
                cardSlot.sprite = cardData.cardBack;
            }
            else if (isAiCardSlot && showingCards)
            {
                cardSlot.sprite = cardData.cardFace;
            }
            UpdateAlpha();
        }
    }

    public void ShowCards()
    {
        if (cardData != null)
        {
            cardSlot.sprite = cardData.cardFace;
        }
        else
            return;
    }
    public void HideCards()
    {
        if (cardData != null)
        {
            cardSlot.sprite = cardData.cardBack;
        }
        else
            return;
    }

    public void UpdateAlpha()
    {

       
    }

    public void DownAlpha()
    {
        cardSlot.color -= new Color(0f, 0f, 0f, 1f);
    }

    public void ClearSlot()
    {
        if (isFull)
        {
            cardSlot.sprite = null;
            cardData = null;
            cardSlot.color = new Color(0f, 0f, 0f, 0f); //test
            cardSlot.enabled = false;
            isFull = false;
            showingCards = false;
        }
    }
}
