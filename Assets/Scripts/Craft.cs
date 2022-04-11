using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Craft : MonoBehaviour
{

    [SerializeField] GameObject CraftingMenu_Canvas;
    [SerializeField] GameObject CraftingItems_List;


    //Icons de cada Item
    [SerializeField] private List<Sprite> ItemsIcon;
    //Lista que contêm todos os slots da janela de crafting (sem contar com a Queue)
    [SerializeField] private List<GameObject> Crafting_Slots;
    //Lista que contêm os prefabs dos items que podem ser craftados
    [SerializeField] private List<GameObject> Crafting_Items;
    //Lista com os materiais disponiveis no jogo
    [SerializeField] private List <GameObject> Materials_List;

    List<bool> crafting_IsValid;

    //um dicionário compost por um objeto que guarda o slot no crafting menu e por outro objeto que guarda o objeto para craftar
    private Dictionary<GameObject, GameObject> ItemsList;

    //um dicionário composto por um int e um game object (quantidade de materiais e objeto do material )
    private Dictionary< List<int>, List<GameObject> > ItemsRecipe;

    //Número de slots da janela de crafting
    [SerializeField] int craftingItems_Number;

    private void Start()
    {
        crafting_IsValid = new List<bool>();
        ItemsRecipe = new Dictionary<List<int>, List<GameObject>>();
        

        int count = 1;

        while (count <= craftingItems_Number)
        {
            foreach (Transform craftingSlot in CraftingItems_List.GetComponentsInChildren<Transform>())
            {
                string slotName = "CraftingSlot" + count.ToString();

                if (count > craftingItems_Number) break;

                if (craftingSlot.name == slotName)
                {

                    //Debug.Log("Name : " + craftingSlot.name);

                    count++;
                    Crafting_Slots.Add(craftingSlot.gameObject);
                    crafting_IsValid.Add(false);
                }

            }
        }

        CreateRecipes();

    }

    void CreateRecipes()
    {

        List<int> recipeNumbers = new List<int>();
        List<GameObject> recipeMaterials = new List<GameObject> ();

        recipeNumbers.Add(1);
        recipeNumbers.Add(1);

        foreach (GameObject material in Materials_List)
        {

            if(material.name == "Wood")
                recipeMaterials.Add(material);

            if (material.name == "Stone")
            {
                recipeMaterials.Add(material);
                break;
            }

        }


        ItemsRecipe.Add(recipeNumbers, recipeMaterials);

    }

    public void OpenCraftingMenu()
    {       

        CraftingMenu_Canvas.SetActive(true);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        CheckIfCanCraft();

    }
    public void CloseCraftingMenu()
    {

        CraftingMenu_Canvas.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    void CheckIfCanCraft()
    {

        for (int i = 0; i < Crafting_Slots.Count; i++)
        {

            if (Crafting_Slots[i] != null)
            {

                bool tempCanCraft = true;
                crafting_IsValid[i] = false;

                int tempRecipe = 0;
                foreach (var recipeItem in ItemsRecipe)
                {
                    if (tempRecipe == i)
                    {
                        for (int j = 0; j < recipeItem.Key.Count; j++)
                        {

                            if (!Inventory.instance.CheckIfCanCraft(recipeItem.Key[j], recipeItem.Value[j]))
                            {

                                tempCanCraft = false;

                            }


                        }

                    }

                    tempRecipe++;

                    if (tempCanCraft)
                    {

                        //TODO :Ativar button e Aumentar Alpha da imagem;


                        Crafting_Slots[i].GetComponentInChildren<Button>().interactable = true;
                        Color tempColor = Crafting_Slots[i].GetComponentInChildren<Image>().color;
                        Debug.Log("Can Craft: " + Crafting_Slots[i].name);

                        Crafting_Slots[i].GetComponentInChildren<Image>().color = new Color(tempColor.r, tempColor.g, tempColor.b, 0.9f);

                        crafting_IsValid[i] = true;
                    }

                }

            }

        }
    }

}
