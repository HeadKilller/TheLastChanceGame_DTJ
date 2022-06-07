using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ZombieBehavior : MonoBehaviour
{
    enum ZombieState
    {
        Idle,
        Chasing,
        Attacking,
        Hit,
        Death
    }


    [SerializeField] private int zombie_InitialHealth;
    [SerializeField] private Animator ZombieAnim;

    [SerializeField] private float zombieSpeed;

    GameObject player;
    Transform playerTransform;
    NavMeshAgent navMeshAgent;

    ZombieState currentZombieState;

    Spawner spawner;
    ZombieSenses zombieSensing;

    bool hasBeenHit;
    private int zombie_CurrentHealth;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        zombieSensing = GetComponentInChildren<ZombieSenses>();

        currentZombieState = ZombieState.Idle;

        player = Player.instance.gameObject;
        playerTransform = player.transform;

        spawner = player.GetComponent<Spawner>();

        zombie_CurrentHealth = zombie_InitialHealth;
        hasBeenHit = false;
    }

    private void Update()
    {
       Debug.Log("Is sensing player : " + zombieSensing.IsSensingPlayer);
        if (zombieSensing.IsSensingPlayer && Vector3.Distance(playerTransform.position, transform.position) >= 1.5f && (currentZombieState == ZombieState.Idle || currentZombieState == ZombieState.Chasing) )
        {
            Debug.Log("Chasing Player");
            ChasePlayer();
        }
        else if(currentZombieState == ZombieState.Chasing && hasBeenHit)
        {
            ChasePlayer();
        }
        else if(Vector3.Distance(playerTransform.position, transform.position) < 1.5f && (currentZombieState != ZombieState.Death /*&& currentZombieState != ZombieState.Hit*/))
        {
            ZombieAnim.SetBool("IsRunning", false);
            ZombieAttack();
        }
        else
        {
            ZombieAnim.SetBool("IsRunning", false);
        }
    }

    private void ZombieAttack()
    {
        currentZombieState = ZombieState.Attacking;
        //Make Zombie Attack

    }

    private void ZombieDeath()
    {
        currentZombieState = ZombieState.Death;

        ZombieAnim.SetTrigger("Zombie_Death");
        gameObject.GetComponent<Collider>().isTrigger = true;

        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        Destroy(gameObject, 5f);
        
    }

    public void ZombieHit(int dmg)
    {

        currentZombieState = ZombieState.Hit;
        hasBeenHit = true;

        navMeshAgent.isStopped = true;
        
        //ZombieAnim.SetBool("IsRunning", false);

        if(!ZombieAnim.GetCurrentAnimatorStateInfo(0).IsName("Zombie Reaction Hit"))
        {
            ZombieAnim.SetTrigger("Zombie_Hit");
        }
        

        zombie_CurrentHealth -= dmg;

        //Debug.Log("Zombie Health: " + zombie_CurrentHealth);

        if (zombie_CurrentHealth <= 0)
        {
            ZombieDeath();
            return;
        }


    }

    public void ChasePlayer()
    {
        if(currentZombieState == ZombieState.Hit)
            navMeshAgent.isStopped = false;


        currentZombieState = ZombieState.Chasing;

        navMeshAgent.destination = playerTransform.position;


        ZombieAnim.SetBool("IsRunning", true);
    }
        

}


