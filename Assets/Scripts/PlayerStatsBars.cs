using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsBars : MonoBehaviour
{

    [SerializeField] Slider healthSider;

    #region Health

    public void SetMaxHealth(int health)
    {

        healthSider.maxValue = health;
        healthSider.value = health;

    }

    public void SetHealth(int health)
    {

        healthSider.value = health;

    }


    #endregion

}
