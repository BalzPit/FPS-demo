using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    float maxHealth = 100;
    float health;

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
        
    }



    /*
     * damage player
     * dmg = damage taken and removed from health pool
     */
    public void TakeDamage(float dmg)
    {
        Debug.Log("Player Damaged! "+ dmg);

        health -= dmg;

        if (health <= 0)
        {
            Debug.Log("Player Dead");
            health = 0;
        }

        //update UI healthbar
        healthBar.setHealth(health);
    }
}
