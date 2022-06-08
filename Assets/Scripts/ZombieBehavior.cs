//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;
//public class ZombieBehavior : MonoBehaviour
//{
//    enum ZombieState
//    {
//        Idle,
//        Chasing,
//        Attacking,
//        Hit,
//        Death
//    }


//    [SerializeField] private int zombie_InitialHealth;
//    [SerializeField] private Animator ZombieAnim;

//    [SerializeField] private float zombieSpeed;

//    [SerializeField] float zombieDamage;


//    [SerializeField] GameObject PunchAttack_CollidesWith;

//    GameObject player;
//    Transform playerTransform;
//    NavMeshAgent navMeshAgent;

//    ZombieState currentZombieState;

//    Spawner spawner;
//    ZombieSenses zombieSensing;

//    bool hasBeenHit;
//    private int zombie_CurrentHealth;

//    private void Start()
//    {
//        navMeshAgent = GetComponent<NavMeshAgent>();
//        zombieSensing = GetComponentInChildren<ZombieSenses>();

//        currentZombieState = ZombieState.Idle;

//        player = Player.instance.gameObject;
//        playerTransform = player.transform;

//        spawner = player.GetComponent<Spawner>();

//        zombie_CurrentHealth = zombie_InitialHealth;
//        hasBeenHit = false;
//    }

//    private void Update()
//    {
//        if (zombieSensing.IsSensingPlayer && Vector3.Distance(playerTransform.position, transform.position) >= 1.5f && (currentZombieState == ZombieState.Idle || currentZombieState == ZombieState.Chasing) )
//        {
//            //Debug.Log("Chasing Player");
//            ChasePlayer();
//        }
//        else if(currentZombieState == ZombieState.Chasing && hasBeenHit)
//        {
//            ChasePlayer();
//        }
//        else if(Vector3.Distance(playerTransform.position, transform.position) < 1.5f && (currentZombieState != ZombieState.Death /*&& currentZombieState != ZombieState.Hit*/))
//        {
//            ZombieAnim.SetBool("IsRunning", false);
//            ZombieAttack();
//        }
//        else
//        {
//            ZombieAnim.SetBool("IsRunning", false);
//        }
//    }

//    private void ZombieAttack()
//    {
//        currentZombieState = ZombieState.Attacking;
//        navMeshAgent.isStopped = true;

//        ZombieAnim.SetTrigger("Zombie_Attack");


//    }

//    public IEnumerator DoDamage()
//    {

//        Debug.Log("Hand : " + PunchAttack_CollidesWith.name);
//        Debug.Log("Collider : " + PunchAttack_CollidesWith.GetComponent<BoxCollider>().enabled);
//        PunchAttack_CollidesWith.GetComponent<BoxCollider>().enabled = true;
//        Debug.Log("Collider : " + PunchAttack_CollidesWith.GetComponent<BoxCollider>().enabled);

//        currentZombieState = ZombieState.Idle;

//        if(Vector3.Distance(playerTransform.position, transform.position) < 1.5f)
//        {

//            ZombieAttack();

//        }
//        else
//        {

//            ChasePlayer();

//        }

//        yield return new WaitForSeconds(2f);

//        PunchAttack_CollidesWith.GetComponent<BoxCollider>().enabled = false;


//    }

//    private void ZombieDeath()
//    {
//        currentZombieState = ZombieState.Death;

//        ZombieAnim.SetTrigger("Zombie_Death");
//        gameObject.GetComponent<Collider>().isTrigger = true;

//        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

//        Destroy(gameObject, 5f);

//    }

//    public void ZombieHit(int dmg)
//    {

//        currentZombieState = ZombieState.Hit;
//        hasBeenHit = true;

//        navMeshAgent.isStopped = true;

//        //ZombieAnim.SetBool("IsRunning", false);

//        if(!ZombieAnim.GetCurrentAnimatorStateInfo(0).IsName("Zombie Reaction Hit"))
//        {
//            ZombieAnim.SetTrigger("Zombie_Hit");
//        }


//        zombie_CurrentHealth -= dmg;

//        //Debug.Log("Zombie Health: " + zombie_CurrentHealth);

//        if (zombie_CurrentHealth <= 0)
//        {
//            ZombieDeath();
//            return;
//        }


//    }

//    public void ChasePlayer()
//    {
//        if(currentZombieState == ZombieState.Hit || currentZombieState == ZombieState.Idle)
//            navMeshAgent.isStopped = false;


//        currentZombieState = ZombieState.Chasing;

//        navMeshAgent.destination = playerTransform.position;


//        ZombieAnim.SetBool("IsRunning", true);
//    }


//}


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

        navMeshAgent = GetComponent<NavMeshAgent>();
        zombieSensing = GetComponentInChildren<ZombieSenses>();

        currentZombieState = Zombie_State.Idle;
        previousZombieState = currentZombieState;

        player = Player.instance.gameObject;
        playerTransform = player.transform;

        spawner = player.GetComponent<Spawner>();

        zombie_CurrentHealth = zombie_InitialHealth;
        hasBeenHit = false;

    }

    private void Update()
    {
        

       
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
            if(Vector3.Distance(transform.position, playerTransform.position) < attackRange)
            {

                Attack();

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

            currentZombieState = Zombie_State.Attacking;

            Attack();

        }

        else
        {

            currentZombieState = Zombie_State.Chasing;
            StartChasingPlayer();

        }

    }

    public void StartChasingPlayer()
    {     

        navMeshAgent.isStopped = false;
        navMeshAgent.destination = playerTransform.position;

        ZombieAnim.SetBool("IsRunning", true);

    }

    public void ChasePlayer()
    {

        navMeshAgent.destination = playerTransform.position;

    }

    public void StopChasingPlayer()
    {

        navMeshAgent.isStopped = true;

        ZombieAnim.SetBool("IsRunning", false);

    }

    public void Attack()
    {
        
        ZombieAnim.SetTrigger("Zombie_Attack");

        StopChasingPlayer();

    }

    public IEnumerator DoDamage()
    {

        PunchAttack_CollidesWith.GetComponent<Collider>().enabled = true;




        yield return new WaitForSeconds(2.12f);

        PunchAttack_CollidesWith.GetComponent<Collider>().enabled = false;

        if (zombieSensing.IsSensingPlayer)
        {

            if(Vector3.Distance(transform.position, playerTransform.position) <= attackRange)
            {

                Attack();

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
        gameObject.GetComponent<Collider>().isTrigger = true;

        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        Destroy(gameObject, 5f);

    }

}
