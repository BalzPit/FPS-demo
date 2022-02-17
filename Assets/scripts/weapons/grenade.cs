using UnityEngine;

public class grenade : MonoBehaviour
{
    float explosion_timer;
    public float explosion_delay;
    public float explosion_radius;
    public float explosion_force;
    public float granade_dmg;

    bool exploding;

    Abilities ab;
    public Rigidbody rb;
    public LayerMask damageable;

    public GameObject explosion_effect;
    public GameObject smoke_effect;

    //AUDIO
    public AudioClip[] bounceSound;
    public AudioClip explosion;

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
                //after enough time has passed, explode
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

        //play random bounce sound
        int randomIndex = Random.Range(0, bounceSound.Length);
        AudioSource.PlayClipAtPoint(bounceSound[randomIndex], transform.position);
    }

    void Explode()
    {
        Debug.Log("Explode");

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosion_radius, damageable);
        Debug.Log("rigidbodies found:" +hitColliders.Length);

        foreach(var hitcollider in hitColliders)
        {
            //always do a constant dmg (granade dmg/4) and add more depending on distance
            float dmg = granade_dmg/4 + granade_dmg * (1 - (transform.position - hitcollider.transform.position).magnitude / explosion_radius);

            if (hitcollider.tag == "Enemy")
            {
                Rigidbody hitRigidBody = hitcollider.GetComponent<Rigidbody>();
                //hitRigidBody.AddExplosionForce(100 * explosion_force, transform.position, explosion_radius);//, 0,ForceMode.Impulse);
                hitRigidBody.AddExplosionForce(explosion_force, transform.position, explosion_radius, 0,ForceMode.Impulse);

                float damage_result = hitcollider.GetComponent<EnemyStatus>().TakeDamage(dmg, hitRigidBody.position-transform.position ,hitRigidBody.position);

                ab.showHitMarker(damage_result);

                //stun enemy
                hitcollider.GetComponent<EnemyStatus>().stunEnemy(dmg);
            }
            else if(hitcollider.tag == "Player")
            {
                hitcollider.GetComponentInParent<PlayerStatus>().TakeDamage(dmg);
            }
        }
        
        Instantiate(explosion_effect, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(explosion, transform.position);

        Destroy(gameObject);
    }

    //link abilities script to this gameobject so we can use its functions
    public void linkAbilities(Abilities abilities)
    {
        ab = abilities;
    }
}
