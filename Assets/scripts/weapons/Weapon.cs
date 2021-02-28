using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This abstract interface is used fo rthe purpose of polymorphism
 * in the single weapons' scripts.
 * All weapons need to perform same basic standars actions, 
 * so having polymorphism helps in the other scripts since they can
 * use the aforementioned actions independently of the equipped weapon.
 */
public abstract class Weapon : MonoBehaviour
{
    public bool reloading;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //check which weapon is equipped
    }

    //ABSTRACT METHODS

    /*
     * cancel reload animation and effect when called
     */
    public abstract void reloadCancel();

    /*
     * randomly move camera position according to recoil direction and strength
     */
    public abstract void weaponRecoil(float recoil_strength, Vector3 ercoil_direction);

}
