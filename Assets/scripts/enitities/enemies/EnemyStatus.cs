using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatus : MonoBehaviour
{
    float health;
    public float explosion_dmg;
    public float explosion_radius;
    public float explosion_force;

    public Rigidbody rb;
    public GameObject deathEffect;
    public LayerMask damageable;
    public Collider self;

    //UI
    //GameObject playerUI;
    public Image hitMarker;
    public Image deatHitMarker;

    // Start is called before the first frame update
    void Start()
    {
        health = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /*
     * damage enemy
     * dmg = damage taken by the object
     * hitPoint = the point where the attack was dealt
     * hitDir = the direction of the impact of the attack
     */
    public float TakeDamage(float dmg, Vector3 hitDirection, Vector3 hitPosition)
    {
        //only deal damage if needed
        if(health > 0)
        {
            Debug.Log("Enemy Damaged! " + dmg);
            health -= dmg;

            if (health <= 0)
            {
                Debug.Log("Enemy Dead");

                //deactivate ai script

                rb.useGravity = true;
                rb.freezeRotation = false;

                rb.AddForceAtPosition(2 * dmg * hitDirection, hitPosition);

                Invoke("Death", 1.5f);

                return -1; //flag value signalling death
            }

            //return taken dmg
            return dmg;
        }

        return 0;
    }



    void Death()
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
}
