using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Item/Equipment")]
public class Equipment : Items
{

    public List<PlayerModifier> modifiers;
    public List<float> modifiersValue;

    public List<Operator> modifiersOperator;

}

public enum PlayerModifier { Attack, Defense, Health, Stamina }
public enum Operator { Multiply, Add }
