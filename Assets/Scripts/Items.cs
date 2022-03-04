using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public abstract class Items
{

    private int id;
    private string name;
    private string description;
    private Image icon;


    private bool isCraftable;
    private bool isStackable;

    private Dictionary<string, int> recipe;
    private Dictionary<string, float> itemModifiers;


    //public Items()
    
}

public class Material : Items
{



}
