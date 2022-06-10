using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieBehavior : MonoBehaviour
{

    enum Zombie_State
    {

        Idle,
        Patroling,
        Chasing,
        Attacking,
        Hit,
        Death

    }

    [SerializeField] private int zombie_InitialHealth;
    [SerializeField] private Animator ZombieAnim;

    [SerializeField] private float zombieSpeed;

    [SerializeField] float zombieDamage;

    [SerializeField] float attackRange = 2f;


    [SerializeField] GameObject PunchAttack_CollidesWith;

    GameObject player;
    Transform playerTransform;
    NavMeshAgent navMeshAgent;

    Zombie_State currentZombieState, previousZombieState;

    Spawner spawner;
    ZombieSenses zombieSensing;

    bool hasBeenHit;
    private int zombie_CurrentHealth;

    private void Start()
    {

        Debug.Log($"Player : {Player.instance.gameObject.name}.");

        player = Player.instance.gameObject;
        playerTransform = player.transform;

        // navMeshAgent = GetComponentInParent<NavMeshAgent>();
        zombieSensing = GetComponentInChildren<ZombieSenses>();

        spawner = player.GetComponent<Spawner>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        currentZombieState = Zombie_State.Idle;
        previousZombieState = currentZombieState;


        zombie_CurrentHealth = zombie_InitialHealth;
        hasBeenHit = false;

    }

    private void Awake()
    {
        



    }

    private void Update()
    {

        //Debug.Log($"Is on navmesh : {navMeshAgent.isOnNavMesh}.");
       
        if(currentZombieState == Zombie_State.Idle)
        {

            if (zombieSensing.IsSensingPlayer)
            {

                previousZombieState = Zombie_State.Idle;
                currentZombieState = Zombie_State.Chasing;

                StartChasingPlayer();

            }


        }

        if (currentZombieState == Zombie_State.Chasing)
        {
            ChasePlayer();

            if(Vector3.Distance(transform.position, playerTransform.position) <= attackRange)
            {
                previousZombieState = Zombie_State.Chasing;
                Attack();

            }
            else if (previousZombieState == Zombie_State.Hit && currentZombieState == Zombie_State.Chasing)
            {

                ChasePlayer();

            }
            else if(!zombieSensing.IsSensingPlayer)
            {

                previousZombieState -= Zombie_State.Chasing;
                currentZombieState = Zombie_State.Idle;
                StopChasingPlayer();

            }

        }

    }

    public void ZombieHit(int dmg)
    {
        StopChasingPlayer();
        zombie_CurrentHealth -= dmg;

        if(zombie_CurrentHealth <= 0f)
        {

            previousZombieState = currentZombieState;
            currentZombieState = Zombie_State.Death;

            Death();

        }
        else
        {

            previousZombieState = currentZombieState;
            currentZombieState = Zombie_State.Hit;

            ZombieAnim.SetTrigger("Zombie_Hit");

        }


    }

    public void FinishHit()
    {

        previousZombieState = currentZombieState;

        if (Vector3.Distance(transform.position, playerTransform.position) <= attackRange)
        {
            previousZombieState = Zombie_State.Hit;
            currentZombieState = Zombie_State.Attacking;

            Attack();

        }

        else
        {
            Debug.Log("Chase Player");

            previousZombieState = Zombie_State.Hit;
            currentZombieState = Zombie_State.Chasing;

            StartChasingPlayer();

        }

    }

    public void StartChasingPlayer()
    {

        currentZombieState = Zombie_State.Chasing;

        
        Debug.Log($"The agent is; {navMeshAgent.name} is in navmesh :  {navMeshAgent.isOnNavMesh}");

        navMeshAgent.isStopped = false;
        navMeshAgent.destination = playerTransform.position;

        ZombieAnim.SetBool("IsRunning", true);

    }

    public void ChasePlayer()
    {

        ZombieAnim.SetBool("IsRunning", true);
        navMeshAgent.destination = playerTransform.position;

    }

    public void StopChasingPlayer()
    {

        navMeshAgent.isStopped = true;

        ZombieAnim.SetBool("IsRunning", false);

    }

    public void Attack()
    {
        
        StopChasingPlayer();

        ZombieAnim.SetTrigger("Zombie_Attack");


    }

    public IEnumerator DoDamage()
    {

        Debug.Log("Start Doing Damage");
        PunchAttack_CollidesWith.GetComponent<Collider>().enabled = true;


        yield return new WaitForSeconds(2.12f);

        Debug.Log("Stop Attack");

        PunchAttack_CollidesWith.GetComponent<Collider>().enabled = false;

        if (zombieSensing.IsSensingPlayer)
        {

            if(Vector3.Distance(transform.position, playerTransform.position) <= attackRange)
            {

                Attack();

            }
            else if(previousZombieState == Zombie_State.Hit)
            {

                StartChasingPlayer();

            }
            else
            {

                currentZombieState = previousZombieState;
                previousZombieState = Zombie_State.Attacking;

                StartChasingPlayer();


            }


        }

        else
        {

            currentZombieState  = Zombie_State.Idle;
            previousZombieState = Zombie_State.Attacking;

        }


    }

    public void Death()
    {

        StopChasingPlayer();

        ZombieAnim.SetTrigger("Zombie_Death");
        

    }

    public void FinishDeathAnim()
    {

        ObjectPooler.instance.DestroyToPool(gameObject);

    }


}
