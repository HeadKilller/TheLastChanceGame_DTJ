using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "Itens/Item")]
public class Items : ScriptableObject
{
    public ItemType itemType;

    new public string name = "New Item";
    public string description = "Description";

    public bool isCraftable;
    public bool isStackable;

    public Sprite icon = null;

    public List<PlayerModifier> modifiers;
    public List<float> modifiersValue;

    public List<Operator> modifiersOperator;

}
public enum PlayerModifier { Attack, Defense, Health, Stamina }
public enum Operator { Multiply, Add }

public enum ItemType { Material, Equipment, Consumible }