using UnityEngine;
using UnityEngine.UI;

public class HealthBarDelay : MonoBehaviour
{
    float previous_health;
    public float delay;
    float time_passed;

    public float slide_time;
    float elapsed;

    bool health_reduced;

    public Slider delaySlider;
    public Slider healthSlider;

    // Start is called before the first frame update
    void Start()
    {
        health_reduced = false;
        time_passed = 0;
        elapsed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (health_reduced)
        {
            //delay
            if (time_passed < delay)
            {
                time_passed += Time.deltaTime;
            }
            else
            {
                //enough time has passed, start decreasing delay slider value
                if (elapsed <= slide_time)
                {
                    //lerp between the starting value and the actual health value
                    setHealth(Mathf.Lerp(previous_health, healthSlider.value, elapsed/slide_time));
                    elapsed += Time.deltaTime;
                }
                else
                {
                    //perfectly sync vaule and stop health loss
                    setHealth(healthSlider.value);
                    health_reduced = false;
                }
            }
        }
    }



    /*
     * health must be a number between 0 and maxhealth
     */
    public void setHealth(float health)
    {
        delaySlider.value = health;
    }

    public void setMaxHealth(float maxHealth)
    {
        delaySlider.maxValue = maxHealth;
        delaySlider.value = maxHealth;
        previous_health = maxHealth;
    }


    public void damaged(float health)
    {
        if (!health_reduced)
        {
            previous_health = health;
            health_reduced = true;

            //reset timers
            time_passed = 0;
            elapsed = 0;
        }
    
        /*put slider where healthbar was if i was in the middle of a delay 
        if (time_passed > 0 && time_passed < delay)
        {
            delaySlider.value = previous_health;
        }
        */
    }
}
