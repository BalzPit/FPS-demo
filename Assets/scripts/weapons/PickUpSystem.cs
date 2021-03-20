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

    float throw_force;
    public float maxforce;
    float kept_velocity_rate = 0.2f;
    public float minDropForwardForce, dropUpwardForce;
    public float pickUpRange;

    public bool equipped;
    public static bool slotFull;

    //calculate velocity of weapon
    Vector3 prev_pos;
    Vector3 current_pos;
    Vector3 velocity;

    RaycastHit rayHit;

    Collider weaponCollider;
    public LayerMask damageable;

    // Start is called before the first frame update
    void Start()
    {
        prev_pos = transform.position;
        current_pos = transform.position;

        throw_force = minDropForwardForce;

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
        float distanceToPlayer = (player.position - transform.position).magnitude;

        //check if player is trying to interact with the weapon
        if (Input.GetKeyDown(KeyCode.E) && distanceToPlayer <= pickUpRange && !equipped && !slotFull)
        {
            if (!wallCheck(distanceToPlayer))
            {
                //pick up weapon
                PickUp();
            }
        }

        //drop weapon
        if (equipped)
        {
            //update positions
            prev_pos = current_pos;
            current_pos = transform.position;

            if (Input.GetKey(KeyCode.Q))
            {
                if (throw_force < maxforce)
                {
                    //increase throw force
                    throw_force += 50*Time.deltaTime;
                }
            }
            //drop/throw weapon
            if (Input.GetKeyUp(KeyCode.Q))
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

        //UI
        gunScript.UIAmmoCounterPickUp();
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
        rb.AddForce(fpsCam.forward * throw_force, ForceMode.Impulse);
        rb.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);
        //add random rotation
        float random_rot = Random.Range(-1f, 1f) * 10;
        rb.AddTorque(new Vector3(random_rot, random_rot, random_rot));

        //UI
        gunScript.UIAmmoCounterDrop();

        gunScript.enabled = false;

        throw_force = minDropForwardForce;
    }


    //throw weapon at enemies
    private void OnCollisionEnter(Collision collision)
    {
        GameObject hitObject = collision.gameObject;
        Debug.Log("weapon collision " + hitObject);

        float dmg = collision.relativeVelocity.magnitude * rb.mass;

        if (hitObject.tag == "Enemy")
        {
            //damage enemy
            hitObject.GetComponent<EnemyStatus>().TakeDamage(dmg, new Vector3(1,1,1) ,collision.GetContact(0).point);

            //bounce back and loose velocity
            rb.velocity = kept_velocity_rate* new Vector3(-rb.velocity.x, -rb.velocity.y, -rb.velocity.z);
        }
    }


    /*
     * return true only if the raycast from this object's transform to the player hits something
     */
    private bool wallCheck(float distance)
    {
        // the ~ is to invert the layermask
        //LayerMask mask = ~damageable;
        //Debug.DrawLine(transform.position, weaponContainer.position, new Color(150, 150, 0), 10f);

        //bool wall = Physics.Raycast(transform.position, transform.position - weaponContainer.position, distance, damageable);
        

        //Debug.Log("wall = "+ wall);
        return false;
    }
}
