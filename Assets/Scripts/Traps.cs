using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum IsTrapPlaced { Placed, NotPlaced , IsBeingPlaced}

public class Traps : ScriptableObject
{
    public int damage;

    public int durability;

    public IsTrapPlaced isTrapPlaced;

    public bool canTrapBePlaced;
}
