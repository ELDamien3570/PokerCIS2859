using TMPro;
using UnityEngine;

public class PotCounter : MonoBehaviour
{
    [Header("Components")]
    public TextMeshProUGUI potText;

    [Header("Variables")]
    public int pot = 0;

    private void Update()
    {
        potText.SetText(pot.ToString());
    }
    public void ResetPot()
    {
        pot = 0;
    }
}
