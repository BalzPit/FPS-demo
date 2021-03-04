using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    public float mouseSensitivity = 100f;

    public Transform playerBody;

    float xRotation = 0f;

    static float walkFov = 60;  //starting default fov
    float sprintFov = walkFov + 5;
    float fov;

    bool isSprinting;

    // Start is called before the first frame update
    void Start()
    {
        //hide cursor during gameplay
        Cursor.lockState = CursorLockMode.Locked;

        //set initial fov
        fov = walkFov;
        Camera.main.fieldOfView = fov;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;


        //float mouseX = Input.GetAxis("Controller X") * mouseSensitivity * Time.deltaTime;
        //float mouseY = Input.GetAxis("Controller Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);  //clamp rotation value so you can't look behind

        //rotate horizontally
        playerBody.Rotate(Vector3.up * mouseX);
        //rotate vertically
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        //smoothly change FOV when sprinting
        if (Input.GetButton("Sprint") && PlayerMovement.sprinting)
        {
            //increase fov when sprinting
            if (fov < sprintFov && (Input.GetAxis("Vertical") != 0))
            {
                fov = Mathf.Lerp(fov, sprintFov, 10 * Time.deltaTime);
                Camera.main.fieldOfView = fov;
            }
        }
        else
        {
            // decrease fov when sprint stops 
            if (fov > walkFov)
            {
                fov = Mathf.Lerp(fov, walkFov, 10 * Time.deltaTime);
                Camera.main.fieldOfView = fov;
            }
        }
    }

    /*
     * rec_dir -> x moves horizontally, y moves vertically
     *
    public static void Recoil(float rec_strength, Vector3 rec_dir, Transform camera)
    {
        //random variation to be added to recoil direction vector
        float r = Random.Range(rec_strength, rec_strength);

        rec_dir += new Vector3(r, r, 0);

        //playerBody.Rotate(Vector3.up * rec_dir.x);
        //camera.localRotation = Quaternion.Euler()
    }*/
}
