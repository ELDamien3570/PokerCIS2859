using UnityEngine;
using UnityEngine.UI;

public class CardValueSlider : MonoBehaviour
{
    Slider slider;

    public int sliderValue;
    private void Start()
    {
        slider = GetComponent<Slider>();
    }
    private void Update()
    {
        if (sliderValue != slider.value)
        {
            slider.value = sliderValue; 
        }
    }
}
