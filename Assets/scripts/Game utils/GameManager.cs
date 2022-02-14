using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    //Asset references
    public GameObject ar;
    public GameObject pistol;
    public GameObject sniper;
    public GameObject shotgun;
    public GameObject enemy;

    //playerReference
    public GameObject runner;

    //script references
    PlayerMovement movementScript;
    Abilities abilitiesScript;
    public GameOverScreen gameOverScreen;
    public UIManager uiManager;

    //score
    int points = 0;

    //enemy spawns
    int enemyCount = 0;
    int maxEnemyCount = 0;
    bool spawning;

    //weapon spawns
    int p = 0;
    static int maxPistols = 5;
    List<GameObject> pistols = new List<GameObject>();
    int pistolCount;

    int a_r = 0;
    static int maxArs = 5;
    List<GameObject> ars = new List<GameObject>();
    int arCount;

    int sn = 0;
    static int maxSnipers = 5;
    List<GameObject> snipers = new List<GameObject>();
    int sniperCount;

    int sg = 0;
    static int maxSg = 5;
    List<GameObject> shotguns = new List<GameObject>();
    int shotgunCount;

    //UI
    Text scoreText;

    //prepare all references needed from the scene
    void Start()
    {
        Time.timeScale = 1;

        //get script instances
        movementScript = FindObjectOfType<PlayerMovement>();
        abilitiesScript = FindObjectOfType<Abilities>();

        //get UI instances
        scoreText = uiManager.getScoreText();

        arCount = 0;
        pistolCount = 0;
        sniperCount = 0;
        shotgunCount = 0;

        //spawn 5 enemies at the start
        nextRound();
    }

    //------------------------------------------------------------------------------- GAME

    //method to be called when the game is over
    public void GameOver()
    {
        //stop player and show game over screen
        movementScript.enabled = false;
        abilitiesScript.enabled = false;

        Cursor.lockState = CursorLockMode.None; //makes menu interactable
        uiManager.gameOver(points);

        //stop game
        Time.timeScale = 0;
    }

    //resets the currently active scene
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //spawns a new wave of enemies
    void nextRound()
    {

        //increase number of enemies in this round
        maxEnemyCount += 5;

        //spawn maxEnemyCount enemies
        for (int i = 0; i < maxEnemyCount; i++)
        {
            int spawnIndex = Random.Range(0, 4);
            spawnEnemy(this.gameObject.transform.GetChild(spawnIndex));
        }
    }

    //------------------------------------------------------------------------------- WEAPONS

    //selects a random weapon from the ones available, depending on the type of enemy
    public GameObject randomWeapon(int enemyType)
    {
        GameObject weapon;

        int r = Random.Range(0, enemyType+1); 

        switch (r)
        {
            case 0:
                weapon = pistol;
                break;
            case 1:
                weapon = ar;
                break;
            case 2:
                weapon = sniper;
                break;
            case 3:
                weapon = shotgun;
                break;
            default:
                weapon = pistol;
                break;
        }

        return weapon;
    }

    //spawn a random weapon depending on the enemy type, and in case there are too many weapons of that type, remove an old one
    void spawnRandomWeapon(Transform spawnPos, int enemyType)
    {
        //GameObject weapon = randomWeapon(enemyType);

        GameObject weapon, weaponToDestroy;

        int r = Random.Range(0, enemyType + 1);

        //MANAGE WEAPONS ON THE GROUND AND DESTROY OLD ONES IF NECESSARY
        //This whole system sucks
        switch (r)
        {
            case 0:
                weapon = Instantiate(pistol, spawnPos.position, Random.rotation);
                
                if(pistolCount == maxPistols)
                {
                    //choose one old weapon to destroy, if it's not equipped
                    bool done = false;

                    while (!done)
                    {
                        if(pistols[p].GetComponent<PickUpSystem>().equipped != true)
                        {
                            weaponToDestroy = pistols[p];
                            //grab instance of new weapon
                            pistols[p] = weapon;

                            //despawn weapon
                            weaponToDestroy.GetComponent<WeaponStatus>().breakWeapon(1);

                            done = true;
                        }

                        p++;

                        if(p >= maxPistols)
                        {
                            p = 0;
                        }
                    }
                }
                else
                {
                    pistols.Add(weapon);
                    pistolCount++;
                }
                break;
            case 1:
                weapon = Instantiate(ar, spawnPos.position, Random.rotation);

                if (arCount == maxArs)
                {
                    //choose one old weapon to destroy, if it's not equipped
                    bool done = false;

                    while (!done)
                    {
                        if (ars[a_r].GetComponent<PickUpSystem>().equipped != true)
                        {
                            weaponToDestroy = ars[a_r];
                            //grab instance of new weapon
                            ars[a_r] = weapon;
                            //despawn weapon
                            weaponToDestroy.GetComponent<WeaponStatus>().breakWeapon(1);
                            
                            done = true;
                        }

                        a_r++;

                        if (a_r == maxArs)
                        {
                            a_r = 0;
                        }
                    }
                }
                else
                {
                    ars.Add(weapon);
                    arCount++;
                }
                break;
            case 2:
                weapon = Instantiate(sniper, spawnPos.position, Random.rotation);

                if (sniperCount == maxSnipers)
                {
                    //choose one old weapon to destroy, if it's not equipped
                    bool done = false;

                    while (!done)
                    {
                        if (snipers[sn].GetComponent<PickUpSystem>().equipped != true)
                        {
                            weaponToDestroy = snipers[sn];
                            //grab instance of new weapon
                            snipers[sn] = weapon;
                            
                            //despawn weapon
                            weaponToDestroy.GetComponent<WeaponStatus>().breakWeapon(1);

                            done = true;
                        }

                        sn++;

                        if (sn == maxSnipers)
                        {
                            sn = 0;
                        }
                    }
                }
                else
                {
                    snipers.Add(weapon);
                    sniperCount++;
                }
                break;
            case 3:
                weapon = Instantiate(shotgun, spawnPos.position, Random.rotation);

                if (shotgunCount == maxSg)
                {
                    //choose one old weapon to destroy, if it's not equipped
                    bool done = false;

                    while (!done)
                    {
                        if (shotguns[sg].GetComponent<PickUpSystem>().equipped != true)
                        {
                            weaponToDestroy = shotguns[sg];
                            //grab instance of new weapon
                            shotguns[sg] = weapon;

                            //despawn weapon
                            weaponToDestroy.GetComponent<WeaponStatus>().breakWeapon(1);

                            done = true;
                        }

                        sg++;

                        if (sg == maxSg)
                        {
                            sg = 0;
                        }
                    }
                }
                else
                {
                    shotguns.Add(weapon);
                    shotgunCount++;
                }
                break;
            default:
                weapon = Instantiate(pistol, spawnPos.position, Random.rotation);

                if (pistolCount == maxPistols)
                {
                    //choose one old weapon to destroy, if it's not equipped
                    bool done = false;

                    while (!done)
                    {
                        if (pistols[p].GetComponent<PickUpSystem>().equipped != true)
                        {
                            weaponToDestroy = pistols[p];
                            //grab instance of new weapon
                            pistols[p] = weapon;

                            //despawn weapon
                            weaponToDestroy.GetComponent<WeaponStatus>().breakWeapon(1);

                            done = true;
                        }

                        p++;

                        if (p == maxPistols)
                        {
                            p = 0;
                        }
                    }
                }
                else
                {
                    pistols.Add(weapon);
                    pistolCount++;
                }
                break;
        }
    }

    public void weaponBroken(int weaponTypeId, GameObject weaponToRemove)
    {
        //This whole system sucks
        switch (weaponTypeId)
        {
            case 0:
                //remove pistol from the list
                pistols.Remove(weaponToRemove);
                pistolCount--;
                break;
            case 1:
                //remove AR from list
                ars.Remove(weaponToRemove);
                arCount--;
                break;
            case 2:
                //remove sniper from list
                snipers.Remove(weaponToRemove);
                sniperCount--;
                break;
            case 3:
                //remove shotgun from list
                shotguns.Remove(weaponToRemove);
                shotgunCount--;
                break;
        }
    }

    //------------------------------------------------------------------------------- PLAYER

    public GameObject getRunnerReference()
    {
        return runner;
    }

    //-------------------------------------------------------------------------------- ENEMY

    //spawn enemy at the spawnpoint's transform
    void spawnEnemy(Transform spawnPoint)
    {
        if (enemyCount < maxEnemyCount)
        {
            GameObject spawnedEnemy = Instantiate(enemy, spawnPoint.position, Random.rotation);
            EnemyStatus enemyScript = spawnedEnemy.GetComponent<EnemyStatus>();

            enemyScript.setGameManager(this);
            //set type of enemy
            enemyScript.setType(Random.Range(0,3));
        }
    }

    //newly instantiated object has to notify the manager
    public void enemySpawned()
    {
        enemyCount++;
    }

    //manager is notified, updates score and spawns a random weapon on enemy's position
    public void enemyDead(Transform enemyTransform, int enemyType, int enemyScore)
    {
        //update score text
        points += enemyScore;
        scoreText.text = "Score: "+ points;

        //spawn random weapon on enemy's last position
        spawnRandomWeapon(enemyTransform, enemyType);

        enemyCount--;

        if (enemyCount == 0)
        {
            nextRound();
        }
    }
}
