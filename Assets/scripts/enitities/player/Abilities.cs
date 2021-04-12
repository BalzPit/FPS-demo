using UnityEngine;

public class Abilities : MonoBehaviour
{
    float delta;

    float max_granade_usages = 2;
    public float granade_throw_force;
    public float granade_cooldown_time;
    float granade_usages;
    float granade_energy;

    public Transform grenadeSpawnPoint;
    public Transform fpsCam;
    public GameObject grenade;

    public GranadeCooldownUI granadeUI;

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

        if (Input.GetButtonDown("granade"))
        {
            throwGranade();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            //grappling hook
            
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
    }



    void throwGranade()
    {
        //only throw if granade is available
        if(granade_usages > 0)
        {
            GameObject grnd = Instantiate(grenade, grenadeSpawnPoint.position, Quaternion.identity);
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
        }
    }
}
