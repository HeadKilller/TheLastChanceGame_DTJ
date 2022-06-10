using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    [SerializeField] float statsLower_Speed;

    [SerializeField] float statsLower_Quantity;
    [SerializeField] MenuHandler menu;


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
    private void Awake()
    {
        instance = this;

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

    public void UseConsumible(Items item)
    {

        Debug.Log("Item consuming : " + item.name);

        List<PlayerModifier> tempModifiers = item.modifiers;


        for(int i = 0; i < tempModifiers.Count; i++)
        {

            switch (tempModifiers[i])
            {

                case PlayerModifier.Health:

                    Heal(item.modifiersValue[i]);

                    break;
                case PlayerModifier.Thirst:

                    Drink(item.modifiersValue[i]);
                    break;
                case PlayerModifier.Hunger:

                    Eat(item.modifiersValue[i]);
                    break;
            }

        }
               
    }


    public void Drink(float _quantity)
    {

        thirst += _quantity;
        if (thirst > 100f)
            thirst = 100f;
        playerBars.SetThirst((int)thirst);

    }

    public void Eat(float _quantity)
    {

        hunger += _quantity;
        if (hunger > 100f)
            hunger = 100f;
        playerBars.SetHunger((int)hunger);

    }

    public void Heal(float _quantity)
    {

        health += _quantity;
        if (health > 100f)
            health = 100f;
        playerBars.SetHealth((int)health);

    }

    public void Damage(float damage)
    {
        health -= damage;

        playerBars.SetHealth((int)health);
        menu.TakeDmgScreen();

    }

    public void Death()
    {
        //Debug.Log("YOU ARE DEATH");
        menu.DeathScreen();
       
    }
}
