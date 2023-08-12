using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Slider slider;
    public Text healthText;
    public void SetMaxBossHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        healthText.text = "Health: " + health;
    }
    public void SetBossHealth(int health)
    {
        slider.value = health;
        healthText.text = "Health: " + health;
    }
}
