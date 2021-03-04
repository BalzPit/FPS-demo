using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camRecoil : MonoBehaviour
{
    public static float rotation_speed;
    public static float return_speed;
    float delta;

    private static Vector3 currentRotation;
    private Vector3 rot;

    private void FixedUpdate()
    {
        delta = Time.deltaTime;

        currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, return_speed * delta);
        rot = Vector3.Slerp(rot, currentRotation, rotation_speed * delta);
        transform.localRotation = Quaternion.Euler(rot);
    }

    // set the recoil rotation vector, rotation speed and return speed
    public static void Fire(Vector3 recoilRotation, float rot_speed, float ret_speed, float rec_force)
    {
        Debug.Log("fire recoil");

        rotation_speed = rot_speed;
        return_speed = ret_speed;
        currentRotation += new Vector3(-recoilRotation.x, Random.Range(-recoilRotation.y, recoilRotation.y), Random.Range(-recoilRotation.z, recoilRotation.z))*50*rec_force;
    }
}
