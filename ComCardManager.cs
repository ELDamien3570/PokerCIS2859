using UnityEngine;

public class ComCardManager : MonoBehaviour
{
    [Header("Components")]
    public CardSlotManager[] cardSlot;

    [Header("Triggers")]
    public bool resetHand;

    public void GetDealtFlop(PlayingCard newCard)
    {
        Sprite newCardSprite = newCard.cardFace;

        for (int i = 0; i <= 2; i++)
        {
            if (!cardSlot[i].isFull)
            {
                cardSlot[i].ChangeSlot(newCardSprite, newCard);
                return;
            }
        }
    }
    public void GetDealtTurn(PlayingCard newCard)
    {
        Sprite newCardSprite = newCard.cardFace;

        
                cardSlot[3].ChangeSlot(newCardSprite, newCard);
       
    }
    public void GetDealtRiver(PlayingCard newCard)
    {
        Sprite newCardSprite = newCard.cardFace;

       
                cardSlot[4].ChangeSlot(newCardSprite, newCard);
        
    }
    public void ResetHand()
    {
        resetHand = false;
        for (int i = 0; i < cardSlot.Length; i++)
        {
            cardSlot[i].ClearSlot();
        }
        
    }
}
