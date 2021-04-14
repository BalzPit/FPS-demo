using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Camera cam;
    public Weapon weapon;
    
    //----------------------FLOATS
    
    //gravity
    float gravity = 3* -9.81f;
    //set player movement speed
    float speed;
    //walk speed
    float walkSpeed = 12f;
    //sprint speed
    float sprintSpeed = 20f;
    //crouching speed
    float crouchSpeed = 7f;
    //sliding speed
    float slideSpeed = 22f;
    //mid-air movement speed
    float airSpeed = 7;
    //jump height
    float jumpH = 3.3f;
    float doublejump_newdir_weight = 1.5f;
    //character controller height
    float standing_height = 3f;
    float crouch_height = 1.5f;
    //slide duration
    float slide_dur = 0.7f;
    //recoil
    public static float rec_movement;

    //sprint transition time
    float sprint_transition_elapsed;
    float sprint_transition_time = 1;

    //----------------------VECTORS

    //store player velocity
    Vector3 velocity;
    //store xz movement
    Vector3 move;
    //midair-enabled movement
    Vector3 air_move;
    //store player movement direction when leaving the grund
    Vector3 air_movement;
    Vector3 slide_movement;
    Vector3 new_direction;
    public static Vector3 recoil_direction;
    /*calculate player velocity
    Vector3 prev_pos;
    Vector3 current_pos;
    public static Vector3 playerVelocity;
    */

    //----------------------BOOLS

    bool onGround;
    bool doubleJump;
    bool crouched, crouch_press, sliding;
    public static bool sprinting;
    public static bool sprint_locked;
    
    //used to check ground presence beneath player
    public Transform groundCheck;
    public float groundDistance = 0.4f; //radius of ground-checking sphere
    public LayerMask groundMask;

    private void Start()
    {
        speed = walkSpeed;
        doubleJump = false;
        sprinting = false;
        sprint_locked = false;
        crouch_press = false;
        crouched = false;
        sliding = false;

        //prev_pos = transform.position;
        //current_pos = transform.position;
        sprint_transition_elapsed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //time delta from last frame
        float delta = Time.deltaTime;
        //check if the player is on the ground
        onGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //gather movement input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //touching the ground
        if (onGround && velocity.y < 0)
        {
            //reset fall velocity
            velocity.y = -8f;
            doubleJump = true;

            //save movement direction
            slide_movement = air_movement;
        }

        /*update positions and compute player velocity
        prev_pos = current_pos;
        current_pos = transform.position;
        playerVelocity = (current_pos - prev_pos) / delta;
        */

        //======================== HORIZONTAL movement ==========================

        // SPRINTING (only if player is on the ground and moving forward)
        //reset timers
        if (Input.GetButtonDown("Sprint") || Input.GetButtonUp("Sprint"))
        {
            sprint_transition_elapsed = 0;
        }

        if (Input.GetButton("Sprint") && onGround && z > 0 && !sprint_locked)
        {
            //smoothly increase speed 
            if (speed < sprintSpeed)
            {
                sprinting = true;
                if (sprint_transition_elapsed < sprint_transition_time)
                {
                    speed = Mathf.Lerp(speed, sprintSpeed, sprint_transition_elapsed/sprint_transition_time);
                    sprint_transition_elapsed += delta;
                }
            }
        }
        else if ((!Input.GetButton("Sprint") || Input.GetButton("Sprint") && z < 0 || sprint_locked) && onGround)
        {
            // smoothly decrease speed 
            if (speed > walkSpeed)
            {
                sprinting = false;
                if (sprint_transition_elapsed < sprint_transition_time)
                {
                    speed = Mathf.Lerp(speed, walkSpeed, sprint_transition_elapsed / sprint_transition_time);
                    sprint_transition_elapsed += delta;
                }
            }
        }

        // if sprinting was locked, re-enable it only if sprint button is released. 
        if (Input.GetButtonUp("Sprint"))
        {
            sprint_locked = false;
        }
        
        // MOVEMENT
        if (onGround)
        {
            if (!sliding)
            {
                //move along x and z axis
                move = transform.right * x + transform.forward * z;
                //move character
                controller.Move(move * speed * delta);

                //store last move value
                air_movement = move;
            }
            else
            {
                //move character in the same direction as when the slide started
                controller.Move(slide_movement * speed * delta);

                //store last move value
                air_movement = slide_movement;
            }
        }
        else
        {
            // give less control IN THE AIR

            //keep moving in the same direction as when player left the ground
            move = air_movement;

            air_move = transform.right * x + transform.forward * z;

            //mid-air horizontal control
            controller.Move(air_move * airSpeed * delta);
            //move character
            controller.Move(move * (speed-airSpeed) * delta);
            
            //controller.Move((air_movement + air_move).normalized * speed * delta);
        }



        //===========================CROUCH===========================

        if (Input.GetButtonDown("crouch"))
        {
            crouch_press = true;
        }
        if (Input.GetButtonUp("crouch"))
        {
            crouch_press = false;
        }

        if(crouch_press && onGround && !sprinting && !crouched && !sliding)
        {
            Crouch();
        }
        else if(crouch_press && onGround && sprinting && !crouched && !sliding)
        {
            Slide();
        }

        if(crouched && !crouch_press && !sliding )
        {
            unCrouch();
        }


        //===========================JUMPING==========================

        if (Input.GetButtonDown("Jump") && onGround)
        {
            //add upward vertical velocity (see velocity formula to reach height jumpH)
            velocity.y = Mathf.Sqrt(jumpH * -2f * gravity);
        }
        else if (Input.GetButtonDown("Jump") && !onGround && doubleJump)
        {
            velocity.y = Mathf.Sqrt(jumpH * -2f * gravity);
            doubleJump = false;

            //store last move value
            float movement_magnitude = air_movement.magnitude;

            //compute angle difference between old and new direction
            new_direction = transform.right * x + transform.forward * z;

            float angle_diff_rate = 1 - ( Vector3.Angle(new_direction, air_movement)/180 ); //the higher the angle difference, the smaller the rate

            //calculate direction of sum of vectors and reapply original magnitude so no speed gained when double jumping
            air_movement = (air_movement + new_direction * doublejump_newdir_weight).normalized * movement_magnitude * angle_diff_rate; 
        }
        
        //reomve RECOIL force
        if(rec_movement != 0)
        {
            //move player with the recoil force
            controller.Move(-recoil_direction * Mathf.Sqrt(rec_movement * -2f * gravity) * delta);

            rec_movement = rec_movement - 10*delta;

            if (rec_movement < 0) rec_movement = 0;
        }

        //fall
        velocity.y += gravity * delta;
        controller.Move(velocity * delta);

    }
    


    private void Crouch()
    {
        Debug.Log("Crouch");
        crouched = true;

        //set height of character controller
        controller.height = crouch_height;
        speed = crouchSpeed;
    }

    

    private void unCrouch()
    {
        Debug.Log("UnCrouch");
        crouched = false;

        //set height of character controller
        controller.height = standing_height;

        //set speed correctly
        if (sprinting)
        {
            speed = sprintSpeed;
        }
        else
        {
            speed = walkSpeed;
        }
    }



    private void Slide()
    {
        Debug.Log("Slidin'");
        Crouch();
        sliding = true;
        speed = slideSpeed;
        Invoke("SlideStop", slide_dur);
    }



    private void SlideStop()
    {
        Debug.Log("SlideStop");
        sliding = false;
        
        //check in which condition to go back to
        if(crouch_press)
        {
            Crouch();
        }
        
    }



    /*
     * other scripts can call this function to stop sprinting when performing other actions
     */
    public static void sprintLock()
    {
        if (Input.GetButton("Sprint"))
        {
            Debug.Log("sprint locked");
            sprint_locked = true;
        }
    }



    public static void Recoil(float rec, Vector3 dir)
    {
        //recoil
        
        Debug.Log("recoil");

        // add velocity in the opposite direction when shooting
        recoil_direction = dir;
        rec_movement = rec;
    }
}
