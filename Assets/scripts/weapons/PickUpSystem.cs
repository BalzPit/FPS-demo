using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpSystem : MonoBehaviour
{
    public Weapon gunScript;
    public Rigidbody rb;
    public BoxCollider coll;
    //GameObject player;
    Transform playerTransform, weaponContainer, fpsCam;

    public Vector3 local_position;
    public Vector3 local_rotation;

    float throw_force;
    public float maxforce;
    public float throwChargingSpeed;
    public float stunDurationMultiplier;
    float kept_velocity_rate = 0.2f;
    public float minDropForwardForce, dropUpwardForce;
    public float pickUpRange;

    public bool equipped;
    public static bool slotFull;

    bool dropBlocked;
    bool chargingThrow;

    //calculate velocity of weapon
    Vector3 prev_pos;
    Vector3 current_pos;
    Vector3 velocity;

    RaycastHit rayHit;

    Collider weaponCollider;

    //UI 
    UIManager uiManager;

    ThrowForceBar throwForceBar;
    Image crosshair;
    public Sprite weaponCrosshair;
    public Sprite defaultCrosshair;

    hitmarker hitmrkr;
    hitmarker deathMarker;

    //Script references
    public WeaponStatus weaponStatusScript;

    //AUDIO
    public AudioClip[] crashSounds;

    //called before start even if script is inactive
    private void Awake()
    {
        //UI
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        //get ui elements
        throwForceBar = uiManager.GetThrowForceBar();
        crosshair = uiManager.getCrosshair();
        hitmrkr = uiManager.getHitMarker();
        deathMarker = uiManager.getDeathHitMarker();

        /*get player transforms
        player = FindObjectOfType<GameManager>().getRunnerReference();

        playerTransform = player.transform;
        weaponContainer = playerTransform.GetChild(2).GetChild(0).GetChild(1);
        fpsCam = playerTransform.GetChild(2).GetChild(0).GetChild(0);*/
    }



    // Start is called before the first frame update
    void Start()
    {
        prev_pos = transform.position;
        current_pos = transform.position;

        throw_force = minDropForwardForce;

        dropBlocked = false;
        chargingThrow = false;

        //initial setup
        if (!equipped)
        {
            gunScript.enabled = false;
            rb.isKinematic = false;
            coll.isTrigger = false;
            slotFull = false; // !!!!!TERRIBLE IDEA!!!! but for now it works because player spawns without guns
        }
        else
        {
            gunScript.enabled = true;
            rb.isKinematic = true;
            coll.isTrigger = true;
            slotFull = true;
        }

        throwForceBar.setMinForce(minDropForwardForce);
        throwForceBar.setMaxForce(maxforce);
        throwForceBar.setForce(0);
    }



    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = (playerTransform.position - transform.position).magnitude;

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
        if (equipped && !dropBlocked)
        {
            //update positions
            prev_pos = current_pos;
            current_pos = transform.position;

            if (Input.GetKey(KeyCode.Mouse1))
            {
                chargingThrow = true;
                if (throw_force < maxforce)
                {
                    //throw force UI
                    throwForceBar.showBar();

                    //increase throw force
                    throw_force += throwChargingSpeed*Time.deltaTime;
                    throwForceBar.setForce(throw_force);
                }
            }
            //drop/throw weapon
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                Drop();
                //throwbar force will be reset to 0 once the bar disappears (look at Update method in throwForceBar class)
                throwForceBar.hideBar();
            }
        }
        else
        {
            if (!rb.IsSleeping())
            {
                //update positions
                prev_pos = current_pos;
                current_pos = transform.position;

                //calculate current velocity
                velocity = (current_pos - prev_pos) / Time.deltaTime;
            }

            //unlock drop capability
            if(dropBlocked && Input.GetKeyUp(KeyCode.Mouse1))
            {
                dropBlocked = false;
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

        //display ammo count
        gunScript.UIAmmoCounterPickUp();

        //display weapon crosshair
        crosshair.sprite = weaponCrosshair;
    }



    public void Drop()
    {
        if (gunScript.reloading)
        {
            gunScript.reloadCancel();
        }

        equipped = false;
        slotFull = false;
        chargingThrow = false;

        //gun not a child of weaponcontainer anymore
        transform.SetParent(null);

        //make rigidbody kinematic and Boxcollider a trigger
        rb.isKinematic = false;
        coll.isTrigger = false;

        //gun carries momentum of player       
        //calculate current velocity
        //velocity = (current_pos - prev_pos) / Time.deltaTime;
        //rb.velocity = velocity;

        //add gun throw force
        rb.AddForce(fpsCam.forward * throw_force, ForceMode.Impulse);
        rb.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);
        //add random rotation
        float random_rot = Random.Range(-1f, 1f) * 10;
        rb.AddTorque(new Vector3(random_rot, random_rot, random_rot));

        //UI
        gunScript.UIAmmoCounterDrop();
        crosshair.sprite = defaultCrosshair;

        gunScript.enabled = false;

        throw_force = minDropForwardForce;
    }



    //throw weapon at enemies
    private void OnCollisionEnter(Collision collision)
    {
        //only deal damage when the weapon hit something because it was moving
        if (velocity != Vector3.zero)
        {

            GameObject hitObject = collision.gameObject;
            Debug.Log("weapon collision " + hitObject);

            float dmg = collision.relativeVelocity.magnitude * rb.mass;

            if (hitObject.tag == "Enemy")
            {
                //damage enemy
                dmg = hitObject.GetComponent<EnemyStatus>().TakeDamage(dmg, new Vector3(1, 1, 1), collision.GetContact(0).point);

                //decrease weapon durability
                weaponStatusScript.durabilityDecrease(dmg);

                //bounce back and loose velocity
                rb.velocity = kept_velocity_rate * new Vector3(-rb.velocity.x, -rb.velocity.y, -rb.velocity.z);

                //stun enemy
                hitObject.GetComponent<EnemyStatus>().stunEnemy(stunDurationMultiplier * dmg);

                //show hitmarker
                if (dmg > 0)
                {
                    //show hitmarker
                    hitmrkr.showHitMarker();
                }
                else if (dmg == -1)
                {
                    //show both hitmarkers
                    hitmrkr.showHitMarker();
                    deathMarker.showHitMarker();
                }
            }
            if (hitObject.tag != "Player")
            {
                //play random crash sound
                int randomIndex = Random.Range(0, crashSounds.Length);
                AudioSource.PlayClipAtPoint(crashSounds[randomIndex], transform.position);
            }
        }
    }



    /*
     * return true if there is something between th eplayer and the game object
     * 
     * distance: the distance from the palyer to the object that's interacting
     */
    private bool wallCheck(float distance)
    {
        bool wall = true;

        bool hit = Physics.Raycast(transform.position, playerTransform.position - transform.position , out rayHit ,distance);

        if(!hit)
        {
            //solves a bug that prevents player from picking up weapon if the player model and the gun model clip in a way that prevents the raycast to hit anything. when this happens, clearly it's because the gun and the player have no wall in between (probably)
            wall = false;
        }
        //no object is in the way, object can be picked up
        else if (rayHit.collider.CompareTag("Player"))
        {
            wall = false;
        }


        //Debug.Log("wall = "+ wall);
        return wall;
    }



    /* sets playerTransform, weaponContainer and fpsCam 
     * p: player transform
     * w: weaponContainer transform
     * c: fpsCam transform
     */
    public void setTransforms(Transform p, Transform w, Transform c)
    {
        playerTransform = p;
        weaponContainer = w;
        fpsCam = c;
    }



    //get weapon equipment status
    public bool getEquippedValue()
    {
        return equipped;
    }

    //resets throw force to 0
    public void interruptDrop()
    {
        if (chargingThrow)
        { 
            dropBlocked = true;

            //hide throw bar
            throwForceBar.hideBar();
        }
        throw_force = 0;
    }
}
