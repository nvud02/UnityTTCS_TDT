using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Slider slider;
    public void SetMaxHP(float health)
    {
        slider.maxValue = health;
        slider.value = health; // Set the initial value to max health
    }

    public void SetHP(float health)
    {
        slider.value = health;
    }
}
