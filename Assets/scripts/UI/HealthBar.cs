using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    float previous_health;

    public Slider healthSlider;
    public HealthBarDelay delay_hb;


    /*
     * health must be a number between 0 and 100
     */
    public void setHealth(float health)
    {
        if (previous_health > health)
        {
            //player lost health
            delay_hb.damaged(previous_health);
        }

        healthSlider.value = health;
        previous_health = health;
    }

    public void setMaxHealth(float maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
        previous_health = maxHealth;

        delay_hb.setMaxHealth(maxHealth);
    }
}
