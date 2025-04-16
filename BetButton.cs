using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BetButton : MonoBehaviour
{
    [Header("Components")]
    public BetCounter betCounter;  
    public PotCounter potCounter;
    public StackCounter stackCounter;
    public GameManager gameManager;
    public PlayerManager playerManager;
    public BetCounter dealerBet;

    [Header("Button Values")]
    public int buttonValue;   

    private void Start()
    {      
        gameManager = FindAnyObjectByType<GameManager>();
        
    }

    public void AddBet()
    {
        if ((betCounter.bet + buttonValue) <= stackCounter.stack)
        betCounter.bet += buttonValue;
    }
    public void ClearBet()
    {
        betCounter.bet = 0;
    }
    public void SubmitBet()
    {
        playerManager.stack -= betCounter.bet;
        gameManager.playerBetReady = false;
        gameManager.playerMadeBet = true;
        gameManager.gameReady = true;
        playerManager.lastBet = betCounter.bet;
        potCounter.pot += betCounter.bet;
    }
    public void FoldButton()
    {
        gameManager.playerFolds = true;
    }
    public void CallButton()
    {
        if (dealerBet.bet != 0)
        {
            betCounter.bet = dealerBet.bet;
            potCounter.pot += betCounter.bet;
            playerManager.stack -= betCounter.bet;

            gameManager.playerCall = true;
            gameManager.playerBetReady = false;
            gameManager.playerMadeBet = true;
            gameManager.gameReady = true;
            playerManager.lastBet = betCounter.bet;
        }
        else
        {
            return;
        }
        
    }
    public void AllInButton()
    {
        betCounter.bet = playerManager.stack;
    }
}
