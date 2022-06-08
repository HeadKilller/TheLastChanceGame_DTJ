using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Gun", menuName = "Itens/Gun")]
public class Guns : Items
{

    public GunType gunType;

    public int magCapacity;
    public int init_MagNum;

    public int rateOfFire;

    public int damage;

    public float maxRange;

    public Vector3 recoil;

}

public enum GunType { None, HandGun, AssaultRifle, SMG }