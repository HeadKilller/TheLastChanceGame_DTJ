using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    [SerializeField] float statsLower_Speed;

    [SerializeField] float statsLower_Quantity;

    public static Player instance;

    public float thirst, hunger, health;

    private float timer, timer01;

    PlayerStatsBars playerBars;

    private void Start()
    {
        instance = this;

        timer = 0f;
        timer01 = 0f;

        health = 100f;
        thirst = 100f;
        hunger = 100;


        playerBars = gameObject.GetComponent<PlayerStatsBars>();

        playerBars.SetMaxHealth((int)health);
        playerBars.SetMaxThirst((int)thirst);
        playerBars.SetMaxHunger((int)hunger);

    }

    private void Update()
    {
        timer += Time.deltaTime;
        timer01 += Time.deltaTime;

        //Reduzir a sede e a fome.
        if(timer >= statsLower_Speed)
        {
            
            hunger -= statsLower_Quantity;
            thirst -= statsLower_Quantity;

            if (hunger < 0f)
                hunger = 0f;
            if(thirst < 0f)
                thirst = 0f;


            timer = 0f;

        }

        //reduzir a vida de 5 em 5 segundos caso fome e/ou sede estejam a 0.
        if ((hunger == 0f || thirst == 0f) && timer01 >= statsLower_Speed)
        {

            health -= 5f;

            timer01 = 0f;
        }

        //senão, caso não esteja a fome ou sede a 0, recuperar vida de 5 em 5 segundos.
        else if(timer01 >= statsLower_Speed && health + 2f <= 100f)
        {

            health += 2f;

            timer01 = 0f;

        }


        if (health <= 0f)
        {
            Death();
        }

        playerBars.SetHealth((int)health);
        playerBars.SetThirst((int)thirst);
        playerBars.SetHunger((int)hunger);


    }

    public void Drink(Items item)
    {

        thirst += 10f;
        playerBars.SetThirst((int)thirst);

    }

    public void Eat(Items item)
    {

        hunger += 10f;
        playerBars.SetHunger((int)hunger);

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

        if(health <= 0f)
        {

            Death();

        }

    }

    public void Death()
    {

        Debug.Log("YOU ARE DEATH");
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name, LoadSceneMode.Single);

    }
}
