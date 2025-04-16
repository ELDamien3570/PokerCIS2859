using UnityEngine;
using System;
using System.Collections;
public class UIManager : MonoBehaviour
{
    [Header("Components")]
    public Canvas bettingButtons;
    public Canvas betCounter;
    public BetCounter betCountScript;
    public Canvas playerWinScreen;
    public Canvas dealerWinScreen;
    public Canvas splitScreen;
    public Canvas dealerBust;
    public Canvas playerBust;
    public Canvas dealerFolds;

    public bool resetWinScreen=false;

    private void Start()
    {
        playerWinScreen.enabled = false;
        splitScreen.enabled = false;
        dealerWinScreen.enabled = false;
        dealerBust.enabled = false;
        playerBust.enabled = false;
        dealerFolds.enabled = false;
    }
    public void openPlayBetting()
    {
        bettingButtons.enabled = true;
       // Debug.Log(bettingButtons.enabled);
        betCounter.enabled = true;
    }
    public void closePlayBetting()
    {
        bettingButtons.enabled = false;
       // Debug.Log(bettingButtons.enabled);
        betCounter.enabled = false;
        betCountScript.bet = 0; 
    }
    public void ShowPlayerWin()
    {
        
        if (!playerWinScreen.isActiveAndEnabled)
        {
            playerWinScreen.enabled = true;
        }
        else
        {
            playerWinScreen.enabled = false;
        }
        StartCoroutine(ResetWinScreen());
    }
    public void ShowDealerWin()
    {
        

        if (!playerWinScreen.isActiveAndEnabled)
        {
            dealerWinScreen.enabled = true;
        }
        else
        {
            dealerWinScreen.enabled = false;
        }
        StartCoroutine(ResetWinScreen());
    }
    public void ShowSplit()
    {
        

        if (!playerWinScreen.isActiveAndEnabled)
        {
            splitScreen.enabled = true;
        }
        else
        {
            splitScreen.enabled = false;
        }
        StartCoroutine(ResetWinScreen());
    }
    public void DealerBust()
    {
       

        if (!playerWinScreen.isActiveAndEnabled)
        {
            dealerBust.enabled = true;
        }
        else
        {
            dealerBust.enabled = false;
        }
        StartCoroutine(ResetWinScreen());
    }
    public void PlayerBust()
    {
        

        if (!playerWinScreen.isActiveAndEnabled)
        {
            playerBust.enabled = true;
        }
        else
        {
            playerBust.enabled = false;
        }
        StartCoroutine(ResetWinScreen());
    }
    public void ShowDealerFolds()
    {


        if (!dealerFolds.isActiveAndEnabled)
        {
            dealerFolds.enabled = true;
        }
        else
        {
            dealerFolds.enabled = false;
        }
        StartCoroutine(ResetWinScreen());
    }
    IEnumerator ResetWinScreen()
    {
        yield return new WaitForSeconds(5);
        playerWinScreen.enabled = false;
        splitScreen.enabled = false;
        dealerWinScreen.enabled = false;
        resetWinScreen = true;
        playerBust.enabled = false;
        dealerBust.enabled= false;
        dealerFolds.enabled = false;
    }
}
