using UnityEngine;

public class Abilities : MonoBehaviour
{
    float delta;

    RaycastHit rayHit;

    public Transform fpsCam;
    public GameObject grenade;

    public PlayerMovement pm;

    //--------------------------GRANADE-----------------
    float max_granade_usages = 2;
    public float granade_throw_force;
    public float granade_cooldown_time;
    float granade_usages;
    float granade_energy;
    public Transform grenadeSpawnPoint;
    GranadeCooldownUI granadeUI;

    //-------------------------GRAPPLING HOOK------------
    public float hookRange;
    public float maxHookTime;
    public float minHookDistance;
    public float hookPullForce;
    public Vector3 hookPosition;
    LineRenderer lineRenderer;

    //UI
    UIManager uiManager;
    hitmarker hitmrkr;
    hitmarker deathMarker;

    //AUDIO
    public AudioClip granadeShot;
    public AudioSource playerSource;

    private void Awake()
    {
        //initialize line renderer object
        lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;

        //UI
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        //get ui elements
        granadeUI = uiManager.GetGranadeCooldownUI();
        hitmrkr = uiManager.getHitMarker();
        deathMarker = uiManager.getDeathHitMarker();
    }

    // Start is called before the first frame update
    void Start()
    {
        granade_usages = max_granade_usages;
        granade_energy = granade_cooldown_time;

        granadeUI.setMaxEnergy(granade_cooldown_time);
        granadeUI.chargeSecondStack();
    }

    // Update is called once per frame
    void Update()
    {
        delta = Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            throwGranade();
        }

        //if (Input.GetButtonDown("Fire2"))
        if(Input.GetKeyUp(KeyCode.C))
        {
            //grappling hook
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out rayHit, hookRange))
            {
                hookPosition = rayHit.point;

                pm.grapplingHook(hookPosition, maxHookTime, minHookDistance, hookPullForce, hookRange);
            }
        }

        //ABILITIES COOLDOWNS

        //GRANADE cooldown
        if (granade_usages < max_granade_usages)
        {
            granade_energy += delta;
            granadeUI.setEnergy(granade_energy, granade_usages);

            if (granade_energy >= granade_cooldown_time)
            {   
                granade_usages++;

                if (granade_usages < max_granade_usages)
                {
                    granade_energy = 0;
                }

                //UI
                if(granade_usages == 2)
                {
                    granadeUI.chargeSecondStack();
                }
            }
        }

        //grappling hook "visuals"
        if (pm.isHooked)
        {
            //draw line between player and hook position
            lineRenderer.SetPosition(0, transform.position); //starting point of the line
            lineRenderer.SetPosition(1, hookPosition); //end point of the line
        }
    }



    void throwGranade()
    {
        //only throw if granade is available
        if(granade_usages > 0)
        {
            GameObject grnd = Instantiate(grenade, grenadeSpawnPoint.position, Quaternion.identity);
            grnd.GetComponent<grenade>().linkAbilities(this);
            Rigidbody rb = grnd.GetComponent<Rigidbody>();

            //add grenade throw force
            rb.AddForce(fpsCam.transform.forward * granade_throw_force, ForceMode.Impulse);

            granade_usages--;

            //start cooldown of a new granade (only if one is not already cooling down)
            if (granade_energy >= granade_cooldown_time)
            {
                granade_energy = 0;
            }

            //UI
            if (granade_usages == 1)
            {
                granadeUI.useSecondStack();
            }
            granadeUI.resetSlider(granade_usages);

            //Play audio clip
            playerSource.PlayOneShot(granadeShot);
        }
    }


    public void showHitMarker(float dmg)
    {
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



    //remove hook visuals
    public void unHook()
    {
        //draw line with same point to hide it
        lineRenderer.SetPosition(0, hookPosition); //starting point of the line
        lineRenderer.SetPosition(1, hookPosition); //end point of the line
    }

}
