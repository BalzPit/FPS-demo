using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    float health;

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
     * damage enemy object
     * dmg = damage taken by the object
     */
    public void TakeDamage(float dmg)
    {
        Debug.Log("Enemy Damaged!");
        health -= dmg;

        if(health <= 0)
        {
            Debug.Log("Enemy Dead");
            Death();
        }
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
