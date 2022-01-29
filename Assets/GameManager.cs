using UnityEngine.SceneManagement;
using UnityEngine;

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

    //prepare all references needed from the scene
    void Start()
    {
        movementScript = FindObjectOfType<PlayerMovement>();
        abilitiesScript = FindObjectOfType<Abilities>();

        //spawn 5 enemies at the start
        nextRound();
    }

    //------------------------------------------------------------------------------- GAME

    //method to be called when the game is over
    public void GameOver()
    {
        //stop player and show game over screen
        Debug.Log("Game over!");
        movementScript.enabled = false;
        abilitiesScript.enabled = false;

        Cursor.lockState = CursorLockMode.None; //makes menu interactable
        uiManager.gameOver();
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

    //spawn a random weapon depending on the enemy type
    void spawnRandomWeapon(Transform spawnPos, int enemyType)
    {
        GameObject weapon = randomWeapon(enemyType);
        Instantiate(weapon, spawnPos.position, Random.rotation);
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

    //manager is notified and spawns a random weapon on enemy's position
    public void enemyDead(Transform enemyTransform, int enemyType)
    {
        //spawn random weapon on enemy's last position
        spawnRandomWeapon(enemyTransform, enemyType);

        enemyCount--;

        if (enemyCount == 0)
        {
            nextRound();
        }
    }
}
