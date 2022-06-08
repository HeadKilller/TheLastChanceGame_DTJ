using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsBars : MonoBehaviour
{

    [SerializeField] Slider healthSlider;
    [SerializeField] Slider thirstSlider;
    [SerializeField] Slider hungerSlider;


    #region Health

    public void SetMaxHealth(int health)
    {

        healthSlider.maxValue = health;
        healthSlider.value = health;

    }

    public void SetHealth(int health)
    {

        healthSlider.value = health;

    }


    #endregion

    #region Thirst

    public void SetMaxThirst(int thirst)
    {

        thirstSlider.maxValue = thirst;
        thirstSlider.value = thirst;

    }

    public void SetThirst(int thirst)
    {

        thirstSlider.value = thirst;

    }

    #endregion

    #region Hunger

    public void SetMaxHunger(int hunger)
    {

        hungerSlider.maxValue = hunger;
        hungerSlider.value = hunger;

    }

    public void SetHunger(int hunger)
    {

        hungerSlider.value = hunger;

    }

    #endregion

}
