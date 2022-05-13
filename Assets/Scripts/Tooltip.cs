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
            Debug.Log("In Inventory");

            string content = itemInInventory_OnHovering.description;
            string header = itemInInventory_OnHovering.name;
       

            TooltipSystem.instance.ToolTip_Show(header, content);
        }
        else if(itemInCraftingSlot_OnHovering != null)
        {
            Debug.Log("Crafting Slot");

            string content = itemInCraftingSlot_OnHovering.description;
            string header = itemInCraftingSlot_OnHovering.name;


            TooltipSystem.instance.ToolTip_Show(header, content);

        }
        else if(itemInCraftingQueueSlot_OnHovering != null)
        {
            Debug.Log("Crafting Queue Slot");

            string content = itemInCraftingQueueSlot_OnHovering.description;
            string header = itemInCraftingQueueSlot_OnHovering.name;


            TooltipSystem.instance.ToolTip_Show(header, content);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.instance.ToolTip_Hide();
    }
}
