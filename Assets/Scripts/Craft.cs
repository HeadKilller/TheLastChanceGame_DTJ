using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Craft : MonoBehaviour
{

    [SerializeField] GameObject CraftingMenu_Canvas;

    [SerializeField] List<GameObject> CraftingMenu_Slots;
    [SerializeField] List<GameObject> CraftingQueue_Slots;

    [SerializeField] List<GameObject> toCraft_Items;
    
    List<GameObject> items_Crafting;
    List<float> craftingTimers;

    int toCraft_Num;
    List<bool> canCraft;


    public static Craft instance;

    private void Start()
    {
     
        instance = this;

        canCraft = new List<bool>();
        craftingTimers = new List<float>();
        items_Crafting = new List<GameObject>();

        toCraft_Num = toCraft_Items.Count;

        //Debug.Log("Num of items to craft : " + toCraft_Num);

        for(int i = 0; i < toCraft_Num; i++)
        {
            canCraft.Add(false);
        }

        foreach(var queue_Slot in CraftingQueue_Slots)
        {
            items_Crafting.Add(null);
            craftingTimers.Add(-100f);
        }


        for(int i = 0; i < toCraft_Num; i++)
        {
            GameObject slotButton = null;

            foreach (var gameobject in CraftingMenu_Slots[i].GetComponentsInChildren<Transform>())
            {

                if (gameobject.name == "SlotButton")
                    slotButton = gameobject.gameObject;

            }

            if(slotButton != null)
            {
                try
                {
                    slotButton.GetComponent<Image>().sprite = toCraft_Items[i].GetComponent<ItemData>().Item.icon;
                }
                catch (System.Exception e)
                {
                    slotButton.GetComponent<Image>().sprite = toCraft_Items[i].GetComponent<ItemData>().Gun.icon;
                }

            }


        }

        

    }

    private void Update()
    {
       

        for(int i = 0; i < craftingTimers.Count; i++)
        {
            if(craftingTimers[i] != -100f)
            {
                craftingTimers[i] -= Time.deltaTime;

                if(craftingTimers[i] <= 0f)
                {

                    craftingTimers[i] = -100f;
                    ItemIsCrafted(CraftingQueue_Slots[i]);

                }
            }
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

            if(tempItem == null)
            {

                tempItem = toCraft_Items[i].GetComponent<ItemData>().Gun;

            }

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

    public void CraftItem(GameObject itemSlot)
    {

        for(int i = 0; i < CraftingMenu_Slots.Count; i++)
        {

            if(CraftingMenu_Slots[i] == itemSlot && canCraft[i])
            {

                List<Items> tempCraftingRecipe;
                List<int> tempRecipeNums;

                Items tempItem = toCraft_Items[i].GetComponent<ItemData>().Item;

                if(tempItem == null)
                    tempItem = toCraft_Items[i].GetComponent<ItemData>().Gun;

                tempCraftingRecipe = tempItem.craftingRecipe;
                tempRecipeNums = tempItem.craftingRecipeNum;


                for(int j = 0; j < tempCraftingRecipe.Count; j++)
                {
                    //if (tempCraftingRecipe[j] == null)
                    //    Debug.Log(tempCraftingRecipe[j].name + " is null.");

                    //Debug.Log("Material : " + tempCraftingRecipe[j]);
                    //Debug.Log("Number of Material : " + tempRecipeNums[j]);

                    Inventory.instance.RemoveItem(tempCraftingRecipe[j], tempRecipeNums[j]);

                }

                Button tempButton = itemSlot.GetComponentInChildren<Button>();

                tempButton.interactable = false;
                var tempColor = tempButton.GetComponent<Image>().color;

                tempColor.a = 30f;
                tempButton.GetComponent<Image>().color = tempColor;

                //Debug.Log("Crafting01... : " + CraftingQueue_Slots.Count);

                for (int j = 0; j < CraftingQueue_Slots.Count; j++)
                {

                    //Debug.Log("Crafting Queue : " + j);

                    if(items_Crafting[j] == null)
                    {
                        //Debug.Log("Crafting...");
                        items_Crafting[j] = toCraft_Items[i];
                        StartCrafting(j);

                        tempButton = CraftingQueue_Slots[j].GetComponentInChildren<Button>();
                        tempButton.GetComponent<Image>().sprite = tempItem.icon;

                        tempColor = tempButton.GetComponent<Image>().color;

                        tempColor.a = 0.9f;
                        tempButton.GetComponent<Image>().color = tempColor;

                        return;
                    }

                }


            }

        }

    }

    private void StartCrafting(int index)
    {

        try
        {
            craftingTimers[index] = items_Crafting[index].GetComponent<ItemData>().Item.craftingTime;
        }
        catch
        {
            craftingTimers[index] = items_Crafting[index].GetComponent<ItemData>().Gun.craftingTime;
        }

    }

    private void ItemIsCrafted(GameObject slot)
    {

        Button tempButton = slot.GetComponentInChildren<Button>();

        tempButton.interactable = true;

        var tempColor = tempButton.GetComponent<Image>().color;
        tempColor.a = 30f;

        tempButton.GetComponent<Image>().color = tempColor;

    }

    public void GetItem(GameObject queue_slot)
    {
        Items tempItem = null;
        GameObject tempGameObject_Prefab = null;

        for(int i = 0; i < CraftingQueue_Slots.Count; i++)
        {
            if(queue_slot == CraftingQueue_Slots[i])
            {
                tempGameObject_Prefab = items_Crafting[i];
                //tempGameObject = toCraft_Items[i];
                tempItem = tempGameObject_Prefab.GetComponent<ItemData>().Item;

                if(tempItem == null)
                    tempItem = tempGameObject_Prefab.GetComponent<ItemData>().Gun;

                items_Crafting[i] = null;

            }
        }

        if(tempGameObject_Prefab != null && tempItem != null)
        {

            GameObject tempGameObject = Instantiate(tempGameObject_Prefab);

            tempGameObject.SetActive(false);

            Inventory.instance.AddItem(tempGameObject, tempItem, tempGameObject,  false);
        }

        Button tempButton = queue_slot.GetComponentInChildren<Button>();
        tempButton.interactable = false;

        Image tempImage = tempButton.GetComponent<Image>();
        tempImage.sprite = null;

        Color tempColor = tempImage.color;
        tempColor.a = 30f;

        tempImage.color = tempColor;

    }


    public Items CheckItemOnHover_CraftingMenu(GameObject craftingSlot)
    {

        Items itemOnHover = null;

        for(int i = 0; i < toCraft_Num; i++)
        {

            if (CraftingMenu_Slots[i] == craftingSlot && toCraft_Items[i] != null)
            {

                itemOnHover = toCraft_Items[i].GetComponent<ItemData>().Item;

                if(itemOnHover == null)
                    itemOnHover = toCraft_Items[i].GetComponent<ItemData>().Gun;
            }


        }

        return itemOnHover;
    }

    public Items CheckItemOnHover_QueueMenu(GameObject craftingQueueSlot)
    {
        Items itemOnHover = null;

        for(int i = 0; i < CraftingQueue_Slots.Count; i++)
        {

            if(CraftingQueue_Slots[i] == craftingQueueSlot && items_Crafting[i] != null)
            {

                itemOnHover = items_Crafting[i].GetComponent<ItemData>().Item;

                if (itemOnHover == null)
                    itemOnHover = items_Crafting[i].GetComponent<ItemData>().Gun;
            }

        }

        return itemOnHover;
    }

    public int CheckItemTimerOnQueue(GameObject slot)
    {

        int time = -1;

        int index = CraftingQueue_Slots.IndexOf(slot);

        time = (int) craftingTimers[index];

        return time;

    }


}
