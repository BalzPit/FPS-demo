using UnityEngine;

public class grenade : MonoBehaviour
{
    float explosion_delay = 2f;
    float explosion_timer;
    bool exploding;

    // Start is called before the first frame update
    void Start()
    {
        exploding = false;
        explosion_timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (exploding)
        {
            explosion_timer += Time.deltaTime;

            if (explosion_timer >= explosion_delay)
            {   
                //after enough time has passed explode
                Explode();
            }
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");
        //start explosion timer
        exploding = true;
    }

    void Explode()
    {
        Debug.Log("Explode");
        Destroy(gameObject);
    }
}
