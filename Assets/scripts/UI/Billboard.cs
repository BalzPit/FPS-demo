using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    float distanceToPlayer;
    public float standardSizeDistance;
    Vector3 stdScale;

    Transform cam;
        
    // Start is called before the first frame update
    void Start()
    {
        //find player camera to look at
        cam = Camera.main.transform;
        stdScale = transform.localScale;
    }

    // LateUpdate is called once per frame after Updates
    void LateUpdate()
    {
        //look towards the camera
        transform.LookAt(transform.position + cam.forward);

        //change size depending on distance from the player (the camera)
        distanceToPlayer = (transform.position - cam.position).magnitude;

        float scaleRatio = distanceToPlayer / standardSizeDistance;
        //clamp value after a certain point
        scaleRatio = Mathf.Clamp(scaleRatio, 1f, 3f);

        transform.localScale = scaleRatio * stdScale;
    }
}
