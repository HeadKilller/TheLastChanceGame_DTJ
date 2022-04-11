using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Craft : MonoBehaviour
{

    [SerializeField] GameObject CraftingMenu_Canvas;
    [SerializeField] GameObject CraftingItems_List;



    [SerializeField] private List<Sprite> ItemsIcon;
    [SerializeField] private List<GameObject> Crafting_Slots;
    [SerializeField] private List<GameObject> Crafting_Items;

    //um dicionário compost por um objeto que guarda o slot no crafting menu e por outro objeto que guarda o objeto para craftar
    private Dictionary<GameObject, GameObject> ItemsList;

    //um dicionário composto por um int e um game object (quantidade de materiais e objeto do material )
    private Dictionary< List<int>, List<GameObject> > ItemsRecipe;
    

    private void Start()
    {       
        

    }

    
    public void OpenCraftingMenu()
    {

        CraftingMenu_Canvas.SetActive(true);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

    }
    public void CloseCraftingMenu()
    {

        CraftingMenu_Canvas.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }



}
