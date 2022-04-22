using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ZombieBehavior : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private int zombie_InitialHealth;
    [SerializeField] private Animator ZombieAnim;

    [SerializeField] private float zombieSpeed;

    NavMeshAgent navMeshAgent;

    ZombieSenses zombieSensing;

    private int zombie_CurrentHealth;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        zombieSensing = GetComponentInChildren<ZombieSenses>();

        zombie_CurrentHealth = zombie_InitialHealth;
    }

    private void Update()
    {
        if (zombieSensing.IsSensingPlayer && Vector3.Distance(playerTransform.position, transform.position) >= 1.5f)
        {
            ZombieAnim.SetBool("IsRunning", true);
            navMeshAgent.destination = playerTransform.position;
        }
        else if(Vector3.Distance(playerTransform.position, transform.position) < 1.5f)
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

        //Make Zombie Attack

    }

    private void ZombieDeath()
    {

        ZombieAnim.SetTrigger("Zombie_Death");
        Destroy(gameObject, 5f);
    }

    public void ZombieHit(int dmg)
    {
        navMeshAgent.isStopped = true;
        
        //ZombieAnim.SetBool("IsRunning", false);

        if(!ZombieAnim.GetCurrentAnimatorStateInfo(0).IsName("Zombie Reaction Hit"))
        {
            ZombieAnim.SetTrigger("Zombie_Hit");
        }
        

        zombie_CurrentHealth -= dmg;

        //Debug.Log("Zombie Health: " + zombie_CurrentHealth);

        if (zombie_CurrentHealth <= 0) ZombieDeath();
                
    }

    public void StartMoving()
    {        
        navMeshAgent.isStopped = false;
        //ZombieAnim.SetBool("IsRunning", true);

    }



}
