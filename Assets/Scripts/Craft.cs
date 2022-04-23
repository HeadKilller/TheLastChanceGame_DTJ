using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Craft : MonoBehaviour
{

    [SerializeField] GameObject CraftingMenu_Canvas;

    [SerializeField] List<GameObject> CraftingMenu_Slots;

    [SerializeField] List<GameObject> toCraft_Items;

    int toCraft_Num;
    List<bool> canCraft;

    private void Start()
    {
        
        canCraft = new List<bool>();

        toCraft_Num = toCraft_Items.Count;

        Debug.Log("Num of items to craft : " + toCraft_Num);

        for(int i = 0; i < toCraft_Num; i++)
        {
            canCraft.Add(false);
        }

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

    private void CheckIfCanCraft()
    {

        //Debug.Log("Checking...");

        for(int i = 0; i < toCraft_Num; i++)
        {

            //Debug.Log("Checking...");

            bool tempCondition = true;

            List<Items> tempCraftingItems = new List<Items>();
            List<int> tempCraftingItemsNum = new List<int>();

            Items tempItem = toCraft_Items[i].GetComponent<ItemData>().Item;

            tempCraftingItems = tempItem.craftingRecipe;
            tempCraftingItemsNum = tempItem.craftingRecipeNum;

            for (int j = 0; j < tempCraftingItems.Count; j++)
            {

                Items tempCraftingMaterial = tempCraftingItems[j];
                int tempCraftingMaterialNum = tempCraftingItemsNum[j];

                if (!Inventory.instance.CheckIfHasInInventory(tempCraftingMaterial, tempCraftingMaterialNum))
                {

                    tempCondition = false;

                }

            }

            canCraft[i] = tempCondition;

        }

        //Debug.Log("Checked");
        
        for(int i = 0; i < toCraft_Num; i++)
        {

            //Debug.Log("Check Craft : " + toCraft_Items[i].name);

            if (canCraft[i])
            {

                //Debug.Log("Can Craft : " + toCraft_Items[i].name);

                Button tempButton = CraftingMenu_Slots[i].GetComponentInChildren<Button>();

                tempButton.interactable = true;
                var tempColor = tempButton.GetComponent<Image>().color;

                tempColor.a = 0.9f;

                tempButton.GetComponent<Image>().color = tempColor;
            }
            else
            {
                //Debug.Log("Can't Craft : " + toCraft_Items[i].name);
            }

        }


    }

}
