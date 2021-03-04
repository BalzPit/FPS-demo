using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    public Weapon gunScript;
    public Rigidbody rb;
    public BoxCollider coll;
    public Transform player, weaponContainer, fpsCam;

    public Vector3 local_position;
    public Vector3 local_rotation;

    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;

    public bool equipped;
    public static bool slotFull;

    //calculate velocity of weapon
    Vector3 prev_pos;
    Vector3 current_pos;
    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        prev_pos = transform.position;
        current_pos = transform.position;

        //initial setup
        if (!equipped)
        {
            gunScript.enabled = false;
            rb.isKinematic = false;
            coll.isTrigger = false;
        }
        else
        {
            gunScript.enabled = true;
            rb.isKinematic = true;
            coll.isTrigger = true;
            slotFull = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 distanceToPlayer = player.position - transform.position;

        //check if player is trying to interact with the weapon
        if (Input.GetKeyDown(KeyCode.E) && distanceToPlayer.magnitude <= pickUpRange && !equipped && !slotFull)
        {
             //pick up weapon
             PickUp();
        }

        //drop weapon
        if (equipped)
        {
            //update positions
            prev_pos = current_pos;
            current_pos = transform.position;

            if (Input.GetKeyDown(KeyCode.Q))
            {
                Drop();
            }
        }
    }



    private void PickUp()
    {
        equipped = true;
        slotFull = true;

        //make weapon a child of the weaponcontainer and move it to default position
        transform.SetParent(weaponContainer);
        transform.localPosition = local_position;
        transform.localRotation = Quaternion.Euler(local_rotation);
        transform.localScale = Vector3.one;

        //make rigidbody kinematic and Boxcollider a trigger
        rb.isKinematic = true;
        coll.isTrigger = true;

        gunScript.enabled = true;
    }



    private void Drop()
    {
        if (gunScript.reloading)
        {
            gunScript.reloadCancel();
        }

        equipped = false;
        slotFull = false;

        //gun not a child of weaponcontainer anymore
        transform.SetParent(null);

        //make rigidbody kinematic and Boxcollider a trigger
        rb.isKinematic = false;
        coll.isTrigger = false;

        //gun carries momentum of player       
        //calculate current velocity
        velocity = (current_pos - prev_pos) / Time.deltaTime;
        rb.velocity = velocity;

        //add gun throw force
        rb.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);
        //add random rotation
        float random_rot = Random.Range(-1f, 1f) * 10;
        rb.AddTorque(new Vector3(random_rot, random_rot, random_rot));

        gunScript.enabled = false;
    }
}
