using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    public Transform grenadeSpawnPoint;
    public Transform fpsCam;
    public GameObject grenade;

    public float forward_force;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            throwGrenade();
        }
    }

    void throwGrenade()
    {
        GameObject grnd = Instantiate(grenade, grenadeSpawnPoint.position, Quaternion.identity);
        Rigidbody rb = grnd.GetComponent<Rigidbody>();      

        //add grenade throw force
        rb.AddForce(fpsCam.transform.forward * forward_force, ForceMode.Impulse);
    }
}
