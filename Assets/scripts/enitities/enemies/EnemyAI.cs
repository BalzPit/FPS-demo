using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    Transform player;

    public LayerMask grondMask, playerMask;

    public EnemyStatus enemyStatus;

    //patroling
    Vector3 walkPoint;
    bool walkpointSet;
    public float walkPointRange;

    //attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //states
    public float sightRange, attackRange;
    bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Runner").transform;
        agent = GetComponent<NavMeshAgent>();

        walkpointSet = false;
    }



    private void Update()
    {
        //AI activates only if the enemy is not stunned
        if (!enemyStatus.isStunned)
        {
            //check for sight and attack range
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerMask);

            if (playerInSightRange)
            {
                playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerMask);

                if (playerInAttackRange)
                {
                    //attack the player
                    attack();
                }
                else
                {
                    //player is in sight range but not in attack range, chase
                    chase();
                }
            }
            else
            {
                //player is not in sight range, normal patrol behaviour
                patrol();
            }
        }
        //do nothing if enemy was stunned
    }


    private void patrol()
    {
        if (!walkpointSet)
        {
            searchWalkPoint();
        }
        else
        {
            //if point is set, go
            agent.SetDestination(walkPoint);

            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            //walkpoint reached
            if(distanceToWalkPoint.magnitude < 1)
            {
                walkpointSet = false;
            }
        }
    }

    private void chase()
    {
        //just run straight towards the player
        agent.SetDestination(player.position);
    }

    private void attack()
    {
        //enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            //attack (explode)
            enemyStatus.Death();

            alreadyAttacked = true;
            Invoke(nameof(resetAttack), timeBetweenAttacks);
        }
    }

    private void searchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, 0, transform.position.z + randomZ);

        //check that point is valid
        if (Physics.Raycast(walkPoint, -transform.up, 2f, grondMask))
        {
            walkpointSet = true;
        }
    }



    private void resetAttack()
    {
        alreadyAttacked = false;
    }
}
