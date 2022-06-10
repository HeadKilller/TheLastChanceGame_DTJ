using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "Itens/Item")]
public class Items : ScriptableObject
{
    public ItemType itemType;
    public GunType munitionsType;

    [Header ("Description")]
    new public string name = "New Item";
    public string description = "Description";
    
    public Sprite icon = null;

    [Header("Bools")]
    public bool isCraftable;
    public bool isStackable;
    public bool isUsable;
    public bool canDrop; // Este bool diz se dropa para o inventario os itens

    [Header("Modifiers")]
    public List<PlayerModifier> modifiers;
    public List<float> modifiersValue;

    public List<Operator> modifiersOperator;

    [Header("Recipe")]
    public List<Items> craftingRecipe;
    public List<int> craftingRecipeNum;

    public float craftingTime;

    [Header("Drops")]
    public string itemDropped;


    public int minDropRate;
    public int maxDropRate;
}
public enum PlayerModifier { Health, Thirst, Hunger }
public enum Operator { Multiply, Add }

public enum ItemType { Material, Equipment, Consumible, Trap, Gun, Radio, Munitions }