using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    static float health;

    // Start is called before the first frame update
    void Start()
    {
        health = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void damagePlayer(float dmg)
    {
        health -= dmg;

        if (health <= 0)
        {
            //dead
            Debug.Log("You Died");
        }
    }
}
