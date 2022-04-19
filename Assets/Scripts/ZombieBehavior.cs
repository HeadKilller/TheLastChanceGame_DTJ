using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBehavior : MonoBehaviour
{
    [SerializeField] private int zombie_InitialHealth;
    //[SerializeField] private Animator ZombieAnim;

    private int zombie_CurrentHealth;

    private void Start()
    {
        zombie_CurrentHealth = zombie_InitialHealth;
    }

    private void Update()
    {

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
