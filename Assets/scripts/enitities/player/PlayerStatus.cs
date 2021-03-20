using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    float delta;

    float maxHealth = 100;
    float health;

    //health recovery
    public float recovery_speed;
    public float recovery_time;
    float recovery_elapsed;

    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthBar.setMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        delta = Time.deltaTime;

        if (recovery_elapsed < recovery_time)
        {
            recovery_elapsed += delta;
        }
        else
        {
            //enough time has passed
            if (health < maxHealth)
            {
                health += delta * recovery_speed;

                if (health> maxHealth)
                {
                    health = maxHealth;
                }

                healthBar.setHealth(health);
            }
        }
    }



    /*
     * damage player
     * dmg = damage taken and removed from health pool
     */
    public void TakeDamage(float dmg)
    {
        Debug.Log("Player Damaged! "+ dmg);

        health -= dmg;

        //reset recovery time
        recovery_elapsed = 0;

        if (health <= 0)
        {
            //dead
            Debug.Log("Player Dead");
            health = 0;

            //do something when player dies
        }

        //update UI healthbar
        healthBar.setHealth(health);
    }
}
