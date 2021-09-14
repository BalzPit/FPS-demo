using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    float health;
    float maxHealth = 100;
    public float explosion_dmg;
    public float explosion_radius;
    public float explosion_force;

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

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthBar.setMaxHealth(maxHealth);

        //set delay time to remove healthbar after death
        removeHealthDelay = healthDelay.slide_time;
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
}
