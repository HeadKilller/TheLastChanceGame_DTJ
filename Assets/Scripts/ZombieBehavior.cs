using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBehavior : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private int zombie_InitialHealth;
    //[SerializeField] private Animator ZombieAnim;

    [SerializeField] private float zombieSpeed;
    
    private int zombie_CurrentHealth;

    private void Start()
    {
        zombie_CurrentHealth = zombie_InitialHealth;
    }

    private void Update()
    {

    }

    public void ZombieSensing()
    {
        //Debug.Log("Update -  curPos:" + transform.position + " target: " + playerTransform.position +
        //    "dist: " + Vector3.Distance(transform.position, playerTransform.position) +
        //    " speed: " + zombieSpeed + " step: " + zombieSpeed * Time.fixedDeltaTime, playerTransform);

        if (Vector3.Distance(playerTransform.position, transform.position) > 1.5)
            MoveTowards();
        
    }

    private void MoveTowards()
    {
        Vector3 targetDirection = playerTransform.position - transform.position;
        Vector3 newDirectionm = Vector3.RotateTowards(transform.forward, targetDirection, 0.7f, 0f);

        transform.rotation = Quaternion.LookRotation(newDirectionm);

        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, zombieSpeed * Time.fixedDeltaTime);
    }

    private void ZombieDeath()
    {

        //ZombieAnim.SetTrigger("Zombie_Death");
        Destroy(gameObject);

    }

    public void ZombieHit(int dmg)
    {

        //ZombieAnim.SetTrigger("Zombie_Hit");

        zombie_CurrentHealth -= dmg;

        Debug.Log("Zombie Health: " + zombie_CurrentHealth);

        if (zombie_CurrentHealth <= 0) ZombieDeath();

    }



}
