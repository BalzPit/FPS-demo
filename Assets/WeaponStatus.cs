﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStatus : MonoBehaviour
{
    float maxDurability;
    public float durability;
    public float shootingDecrease; //amount that determines by how much weapon deteriorates after each shot

    bool criticalDamage;
    float criticalThreshold = 0.25f; //percentage at which damage becomes critical 

    //script reference
    public PickUpSystem pickupScript;

    //asset reference
    public GameObject breakEffect;
    public GameObject smokeTrail;

    //transform reference
    public Transform smokeTransform;

    //setter method
    public void setDurability(float dur)
    {
        durability = dur;
    }

    //getter method
    public float getDurability()
    {
        return durability;
    }

    public void durabilityDecrease(float amount)
    {
        durability -= amount;

        //check if weapon broke
        if (durability <= 0)
        {
            breakWeapon();
        }
        else if (durability < criticalThreshold*maxDurability && !criticalDamage)
        {
            //durability has become critical after taking damage
            criticalDamage = true;
            Instantiate(smokeTrail, smokeTransform.position, Quaternion.identity, smokeTransform);
        }
    }

    //destroy weapon when it's broken
    void breakWeapon()
    {
        //set slot as empty if weapon was equipped
        if (pickupScript.getEquippedValue() == true)
        {
            //drop weapon
            pickupScript.Drop();
            //PickUpSystem.slotFull = false;
        }

        //destroy weapon gameObject
        Destroy(gameObject);
        GameObject be = Instantiate(breakEffect, transform.position, Quaternion.identity);
        be.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);//sccale explosion down
    }

    private void Start()
    {
        //set the initial value for durability
        maxDurability = durability;
        criticalDamage = false;
    }
}
