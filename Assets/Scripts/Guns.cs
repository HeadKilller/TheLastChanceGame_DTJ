using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Gun", menuName = "Itens/Gun")]
public class Guns : ScriptableObject
{

    public GunType gunType;

    public int magCapacity;
    public int init_MagNum;

    public int rateOfFire;

    public int damage;

    public float maxRange;

}

public enum GunType { HandGun, AssaultRifle }