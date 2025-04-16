using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "PlayingCards")]
public class PlayingCard : ScriptableObject
{
    public Sprite cardFace;
    public Sprite cardBack;

    public int cardValue;
    public Suit cardSuit;

    public bool dealtThisHand;

    public enum Suit
    {
        Spade,
        Club,
        Heart,
        Diamond
    }
}
