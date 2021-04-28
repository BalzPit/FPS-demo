using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        //find player camera to look at
        cam = Camera.main.transform;
    }

    // LateUpdate is called once per frame after Updates
    void LateUpdate()
    {
        //look towards the camera
        transform.LookAt(transform.position + cam.forward);
    }
}
