using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController UIControl;

    public static GameObject hitMarker;
    public static GameObject hitMarkerDeath;

    /**
     *  create a singleton static instance of the class to be use everywhere 
     */
    void Awake()
    {
        if (UIControl != null)
            GameObject.Destroy(UIControl);
        else
            UIControl = this;

        DontDestroyOnLoad(this);
    }

    /**
     * show hitmarkers from anywhere
     */
    public static void showHitmarker()
    {
        hitMarker.GetComponent<hitmarker>().showHitMarker();
    }
    public static void showHitmarkerDeath()
    {
        hitMarkerDeath.GetComponent<hitmarker>().showHitMarker();
    }
}
