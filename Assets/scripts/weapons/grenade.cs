using UnityEngine;

public class grenade : MonoBehaviour
{
    float explosion_delay = 2f;
    float explosion_timer;
    bool exploding;

    public Rigidbody rb;
    //public Transform effects;

    public GameObject explosion_effect;
    public GameObject smoke_effect;

    // Start is called before the first frame update
    void Start()
    {
        exploding = false;
        explosion_timer = 0;

        Instantiate(smoke_effect, transform.position, Quaternion.identity, transform);
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
        if (!exploding)
        {
            Debug.Log("Collision");
            //start explosion timer
            exploding = true;

            //decrease grenade velocity on impact to make it more predictable
            rb.velocity *= 0.5f;
        }
    }

    void Explode()
    {
        Debug.Log("Explode");

        Instantiate(explosion_effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
