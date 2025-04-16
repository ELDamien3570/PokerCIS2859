using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StackCounter : MonoBehaviour
{
    [Header("Components")]
    public TextMeshProUGUI stackText;
    public PlayerManager playerManager;
    public DealerManager dealerManager;
    public GameManager gameManager;
    
    [Header("Variables")]
    public int stack;

    public bool aiStack;

    private void Start()
    {
        stack = playerManager.stack;
        gameManager = FindFirstObjectByType<GameManager>();
    }
    private void Update()
    {
        if (!aiStack)
        {
            stack = playerManager.stack;
        }
        else if (aiStack)
        {
            stack = dealerManager.aiStack;
        }
        stackText.SetText(stack.ToString());
        if (stack < 5)
        {
            gameManager.playerAllIn = true;
        }
        else
        {
            gameManager.playerAllIn = false;
        }
        
            //playerManager.stack = stack;      
    }
    public void GetWinning(int winnings)
    {
        if (!aiStack)
        {
            playerManager.stack = playerManager.stack + winnings;
            Debug.Log("Received winnings" + winnings);
        }
        else if (aiStack)
        {
            dealerManager.aiStack += winnings;
        }
    }
}