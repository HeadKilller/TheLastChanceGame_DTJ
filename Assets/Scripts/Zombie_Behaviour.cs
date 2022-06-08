using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie_Behaviour : MonoBehaviour
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

    [SerializeField] float attackingDistance = 2f;


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
        
        if ( zombieSensing.IsSensingPlayer && currentZombieState == Zombie_State.Idle )
        {

            previousZombieState = currentZombieState;
            currentZombieState = Zombie_State.Chasing;

        }

        if (Vector3.Distance(transform.position, playerTransform.position) <= attackingDistance && (currentZombieState != Zombie_State.Attacking || currentZombieState != Zombie_State.Hit || currentZombieState != Zombie_State.Death) )
        {

            previousZombieState = currentZombieState;
            currentZombieState = Zombie_State.Attacking;

        }

        if (zombie_CurrentHealth <= 0f && currentZombieState != Zombie_State.Death)
        {

            currentZombieState = Zombie_State.Death;

        }

        if(currentZombieState != Zombie_State.Death && currentZombieState != Zombie_State.Attacking && previousZombieState == Zombie_State.Hit)
        {

            previousZombieState = currentZombieState;
            currentZombieState = Zombie_State.Chasing;

        }

        if(currentZombieState == Zombie_State.Attacking && zombieSensing.IsSensingPlayer)
        {

            currentZombieState = previousZombieState;
            previousZombieState = Zombie_State.Attacking;

        }


        if(previousZombieState == Zombie_State.Chasing && currentZombieState != Zombie_State.Chasing)
        {

            StopChasingPlayer();

        }

        if(previousZombieState != Zombie_State.Death && currentZombieState == Zombie_State.Chasing)
        {

            StartChasingPlayer();

        }

        if(currentZombieState == Zombie_State.Attacking)
        {

            Attack();

        }

    }

    public void ZombieHit(int dmg)
    {

        previousZombieState = currentZombieState;
        currentZombieState = Zombie_State.Hit;

        zombie_CurrentHealth -= dmg;

    }

    public void StartChasingPlayer()
    {

        navMeshAgent.isStopped = false;
        navMeshAgent.destination = playerTransform.position;

        ZombieAnim.SetBool("IsRunning", true);

    }

    public void StopChasingPlayer()
    {

        navMeshAgent.isStopped = true;

        ZombieAnim.SetBool("IsRunning", false);

    }

    public void Attack()
    {

        ZombieAnim.SetTrigger("Zombie_Attack");

    }

    public IEnumerator DoDamage()
    {

        PunchAttack_CollidesWith.GetComponent<Collider>().enabled = true;




        yield return new WaitForSeconds(1.5f);

        PunchAttack_CollidesWith.GetComponent<Collider>().enabled = false;

    }

}
