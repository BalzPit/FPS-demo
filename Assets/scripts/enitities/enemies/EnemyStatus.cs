using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    int enemyType;
    int enemyPoints=0;

    float health;
    float maxHealth = 100;
    float explosion_dmg = 50;
    float explosion_radius = 5;
    float explosion_force = 5;

    public Rigidbody rb;
    public GameObject deathEffect;
    public LayerMask damageable;
    public Collider self;

    //UI

    //ENEMY HEALTH
    public HealthBar healthBar;
    public HealthBarDelay healthDelay;
    public Canvas healthCanvas;
    float removeHealthDelay;

    //AI
    public EnemyAI ai;

    //gamemanager reference
    GameManager manager;

    //AUDIO
    public AudioSource explosionSound;

    // Start is called before the first frame update
    void Start()
    {
        //default settings
        health = maxHealth;
        healthBar.setMaxHealth(maxHealth);

        //set delay time to remove healthbar after death
        removeHealthDelay = healthDelay.slide_time;

        //notify manager of successfull instantiation
        manager.enemySpawned();
    }



    /*
     * damage enemy
     * 
     * dmg = damage taken by the object
     * hitPoint = the point where the attack was dealt
     * hitDir = the direction of the impact of the attack
     * 
     * Returns: amount of damage taken or -1 when the damage reduces health to or below 0
     */
    public float TakeDamage(float dmg, Vector3 hitDirection, Vector3 hitPosition)
    {
        //only deal damage if needed
        if(health > 0)
        {
            Debug.Log("Enemy Damaged! " + dmg);
            health -= dmg;

            //reduce health
            healthBar.setHealth(health);

            if (health <= 0)
            {
                Debug.Log("Enemy Dead");

                //deactivate ai script

                rb.useGravity = true;
                rb.freezeRotation = false;

                rb.AddForceAtPosition(2 * dmg * hitDirection, hitPosition);

                ai.enabled = false;

                Invoke("Death", 1.5f);

                Invoke("removeHealthbar", removeHealthDelay);

                return -1; //flag value signalling death
            }

            //return taken dmg
            return dmg;
        }

        return 0;
    }



    public void Death()
    {
        Explode();
        Instantiate(deathEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);

        //notify gamemanager of death
        manager.enemyDead(transform, enemyType, enemyPoints);
    }



    void Explode()
    {
        Debug.Log("Explode");

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosion_radius, damageable);
        Debug.Log("rigidbodies found:" + hitColliders.Length);

        foreach (var hitcollider in hitColliders)
        {
            //decreasing damage depending on distance
            float dmg = explosion_dmg * (1 - (transform.position - hitcollider.transform.position).magnitude / explosion_radius);

            if (hitcollider.tag == "Enemy")
            {
                if (hitcollider != self)
                {
                    Rigidbody hitRigidBody = hitcollider.GetComponent<Rigidbody>();
                    hitRigidBody.AddExplosionForce(100 * explosion_force, transform.position, explosion_radius);

                    hitcollider.GetComponent<EnemyStatus>().TakeDamage(dmg, transform.position-hitRigidBody.position, hitRigidBody.position);
                }
            }
            else if (hitcollider.tag == "Player")
            {
                hitcollider.GetComponentInParent<PlayerStatus>().TakeDamage(dmg);
            }
        }

        //play explosion sound
        AudioSource.PlayClipAtPoint(explosionSound.clip, transform.position);
    }



    /*
     * fade the healthbar out
     */
    public void removeHealthbar()
    {
        //remove healthbar
        healthCanvas.enabled = false;
    }



    /*
     * show the healthbar
     */
    public void showHealthbar()
    {
        //show healthbar
        healthCanvas.enabled = true;
    }



    //take gamemanager reference
    public void setGameManager(GameManager m)
    {
        manager = m;
    }



    //set type of enemy, health, size, damage and weapons dropped will change according to the type
    public void setType(int type)
    {
        enemyType = type;

        //there are 3 types
        switch (type)
        {
            case 0:
                //small enemy
                maxHealth = 100;
                explosion_dmg = 50;
                explosion_radius = 5;
                explosion_force = 5;
                enemyPoints = 5;
                //no change to scale
                break;
            case 1:
                //medium enemy
                maxHealth = 200;
                explosion_dmg = 70;
                explosion_radius = 7;
                explosion_force = 10;
                enemyPoints = 10;
                gameObject.GetComponent<Transform>().localScale += new Vector3(1, 1, 1); //1 = same as type
                break;
            case 2:
                //big enemy
                maxHealth = 400;
                explosion_dmg = 90;
                explosion_radius = 12;
                explosion_force = 15;
                enemyPoints = 20;
                gameObject.GetComponent<Transform>().localScale += new Vector3(2, 2, 2); //2 = same as type
                break;
            default:
                break;
        }

        //set selected health
        health = maxHealth;
        healthBar.setMaxHealth(maxHealth);
    }
}
