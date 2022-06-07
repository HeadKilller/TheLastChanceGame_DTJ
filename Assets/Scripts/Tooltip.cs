using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{



    public void OnPointerEnter(PointerEventData eventData)
    {
        Items itemInInventory_OnHovering = Inventory.instance.CheckItemOnHover(gameObject);
        Items itemInCraftingSlot_OnHovering = Craft.instance.CheckItemOnHover_CraftingMenu(gameObject);
        Items itemInCraftingQueueSlot_OnHovering = Craft.instance.CheckItemOnHover_QueueMenu(gameObject);

        if(itemInInventory_OnHovering != null)
        {
            //Debug.Log("In Inventory");

            string content = itemInInventory_OnHovering.description;
            string header = itemInInventory_OnHovering.name;
       

            TooltipSystem.instance.ToolTipInventory_Show(header, content);
        }
        else if(itemInCraftingSlot_OnHovering != null)
        {
            //Debug.Log("Crafting Slot");

            string materials = "";

            string header = itemInCraftingSlot_OnHovering.name + " Recipe";

            List<Items> materialsNames = itemInCraftingSlot_OnHovering.craftingRecipe;
            List<int> materialsNum = itemInCraftingSlot_OnHovering.craftingRecipeNum;

            

            for(int i = 0; i < materialsNum.Count; i++)
            {

                materials += materialsNames[i].name + "          " + materialsNum[i].ToString() +  "\n";

            }


            TooltipSystem.instance.ToolTipCrafting_Show(header, materials);

        }
        else if(itemInCraftingQueueSlot_OnHovering != null)
        {
            //Debug.Log("Crafting Queue Slot");

            string content = Craft.instance.CheckItemTimerOnQueue(gameObject).ToString() + " sec";
            string header = itemInCraftingQueueSlot_OnHovering.name + " Time Left : ";


            TooltipSystem.instance.ToolTipInventory_Show(header, content);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.instance.ToolTip_Hide();
    }
}
