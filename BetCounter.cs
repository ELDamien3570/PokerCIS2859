using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BetCounter : MonoBehaviour
{
    [Header("Components")]
    public TextMeshProUGUI betText;

    [Header("Variables")]
    public int bet = 0;



     private void Update()
    {
        betText.SetText(bet.ToString());
    }
}
