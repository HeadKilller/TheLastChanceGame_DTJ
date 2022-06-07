using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player instance;

    public float thirst, hunger, health;

    private float timer;

    PlayerStatsBars playerBars;

    private void Start()
    {
        instance = this;

        timer = 0f;

        health = 100f;
        thirst = 100f;
        hunger = 100;


        playerBars = gameObject.GetComponent<PlayerStatsBars>();

        playerBars.SetMaxHealth((int)health);
    }

    private void Update()
    {
        timer += Time.deltaTime;   
        
        //Reduzir a sede e a fome.
        if(timer % 5f == 0)
        {
            
            hunger -= 1f;
            thirst -= 1f;

            if (hunger < 0f)
                hunger = 0f;
            if(thirst < 0f)
                thirst = 0f;
        }

        //reduzir a vida de 5 em 5 segundos caso fome e/ou sede estejam a 0.
        if((hunger == 0f || thirst == 0f) && timer % 5f == 0)
        {

            health -= 5f;

        }

        //senão, caso não esteja a fome ou sede a 0, recuperar vida de 5 em 5 segundos.
        else if(timer % 5 == 0 && health + 2f <= 100f)
        {

            health += 2f;

        }


        if(health <= 0f)
        {
            Death();
        }

        playerBars.SetHealth((int)health);


    }

    public void Drink(Items item)
    {

        thirst += 10f;

    }

    public void Eat(Items item)
    {

        hunger += 10f;

    }

    public void Heal(Items item)
    {

        health += 20f;
        playerBars.SetHealth((int)health);

    }

    public void Damage(float damage)
    {
        health -= damage;
        playerBars.SetHealth((int)health);

    }

    public void Death()
    {

        Debug.Log("YOU ARE DEATH");
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name, LoadSceneMode.Single);

    }
}
