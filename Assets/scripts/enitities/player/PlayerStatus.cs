using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    float delta;

    float maxHealth = 100;
    float health;

    //damage multiplier
    float dmgMulti = 1;
    bool buffed = false;
    public float buffTime; //set in editor
    float buffTimeElapsed;

    //health recovery
    public float recovery_speed;
    public float recovery_time;
    float recovery_elapsed;

    //UI
    UIManager uiManager;
    HealthBar healthBar;

    private void Awake()
    {
        //UI
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        //get ui coponents
        healthBar = uiManager.getHealthBar();
    }

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

        //buff time countdown
        if(buffed == true)
        {
            buffTimeElapsed += delta;

            if (buffTimeElapsed >= buffTime)
            {
                buffed = false;
                dmgMulti = 1;
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
            FindObjectOfType<GameManager>().GameOver();
        }

        //update UI healthbar
        healthBar.setHealth(health);
    }

    /* buff player damage
     * amount: multiplier of the buff (amount = 1 -> 100% buff) 
     */
    public void buffDmgMultiplier(float amount)
    {
        dmgMulti += amount;
        //start countdown to remove buff
        buffTimeElapsed = 0;
        buffed = true;
    }

    //return the damage multiplier value the player currently holds
    public float getDmgMultiplier()
    {
        return dmgMulti;
    }
}
