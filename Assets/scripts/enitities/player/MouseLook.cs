using UnityEngine;

public class MouseLook : MonoBehaviour
{
    float delta;

    public float mouseSensitivity = 100f;

    float xRotation = 0f;

    static float walkFov = 60;  //starting default fov
    float sprintFov = walkFov + 5;
    float fov;

    float fov_transition_time = 1;
    float fov_transition_elapsed;

    bool isSprinting;

    public Transform playerBody;
    Camera fpsCamera;

    // Start is called before the first frame update
    void Start()
    {
        fpsCamera = Camera.main;
        //hide cursor during gameplay
        Cursor.lockState = CursorLockMode.Locked;

        //set initial fov
        fov = walkFov;
        fpsCamera.fieldOfView = fov;
        fov_transition_elapsed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        delta = Time.deltaTime;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * delta;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * delta;


        //float mouseX = Input.GetAxis("Controller X") * mouseSensitivity * Time.deltaTime;
        //float mouseY = Input.GetAxis("Controller Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);  //clamp rotation value so you can't look behind

        //rotate horizontally
        playerBody.Rotate(Vector3.up * mouseX);
        //rotate vertically
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        //smoothly change FOV when sprinting
        if (Input.GetButtonDown("Sprint") || Input.GetButtonUp("Sprint"))
        {
            fov_transition_elapsed = 0;
        }

        if (Input.GetButton("Sprint") && PlayerMovement.sprinting)
        {
            //increase fov when sprinting
            if (fov < sprintFov && (Input.GetAxis("Vertical") != 0))
            {
                if (fov_transition_elapsed < fov_transition_time)
                {
                    fov = Mathf.Lerp(fov, sprintFov, fov_transition_elapsed / fov_transition_time);
                    fov_transition_elapsed += delta;
                    fpsCamera.fieldOfView = fov;
                }
            }
        }
        else
        {
            // decrease fov when sprint stops 
            if (fov > walkFov)
            {
                if (fov_transition_elapsed < fov_transition_time)
                {
                    fov = Mathf.Lerp(fov, walkFov, fov_transition_elapsed / fov_transition_time);
                    fov_transition_elapsed += delta;
                    fpsCamera.fieldOfView = fov;
                }
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
