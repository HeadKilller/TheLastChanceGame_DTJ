using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler
{
    Vector3 defaultPosition;

    List<GameObject> availableItemsSlots;
    List<Items> slotsContent;

    GameObject invPanel, gunsWheelPanel;

    List<RectTransform> invSlotsTransform, gunsWheelSlotsTransform;

    GameObject objectDragging = null;

    public void OnDrag(PointerEventData eventData)
    {
        string[] splited = this.name.Split(' ');
        int index = -1;

        try
        {
            index = Int32.Parse(splited[1]);
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
        }

        if (index != -1)
        {
            objectDragging = availableItemsSlots[index - 1];
        }

        transform.position = Input.mousePosition;

    }

    public void OnEndDrag(PointerEventData eventData)
    {

        RectTransform invPanelTransform = invPanel.transform as RectTransform;
        RectTransform gunsWheelPanelTransform = gunsWheelPanel.transform as RectTransform; 
        
        if(RectTransformUtility.RectangleContainsScreenPoint(invPanelTransform, Input.mousePosition))
        {

            for(int i = 0; i < invSlotsTransform.Count; i++)
            {

                if (RectTransformUtility.RectangleContainsScreenPoint(invSlotsTransform[i], Input.mousePosition))
                {

                    Debug.Log("Add Item to inventory");


                    Inventory.instance.RemoveItem(objectDragging, true, availableItemsSlots[i]);

                }

            }
                        

        }
        if (RectTransformUtility.RectangleContainsScreenPoint(gunsWheelPanelTransform, Input.mousePosition))
        {
            //for (int i = 0; i < gunsWheelSlotsTransform.Count; i++)
            //{

            //    if (RectTransformUtility.RectangleContainsScreenPoint(gunsWheelSlotsTransform[i], Input.mousePosition))
            //    {

            //        Debug.Log("Add Item to inventory");

            //        Inventory.instance.RemoveItem(this.gameObject, true, availableItemsSlots[i]);

            //    }

            //}
            

        }
        //for(int i = 0; i < availableItemsSlots.Count; i++)
        //{


        //}        

        transform.localPosition = defaultPosition;
    }

    // Start is called before the first frame update
    void Start()
    {


        defaultPosition = transform.localPosition;

        slotsContent = Inventory.instance.Inventory_SlotsContent;
        availableItemsSlots = Inventory.instance.Inventory_Slots;

        invPanel = Inventory.instance.InvPanel;
        gunsWheelPanel = Inventory.instance.GunsWheelPanel;

        invSlotsTransform = new List<RectTransform>();
        gunsWheelSlotsTransform = new List<RectTransform>();

        foreach (var child in invPanel.GetComponentsInChildren<Transform>())
        {

            if (child.name.Contains("InventorySlot"))
            {
                invSlotsTransform.Add(child.transform as RectTransform);
            }

        }
        foreach (var child in gunsWheelPanel.GetComponentsInChildren<Transform>())
        {

            if (!child.name.Contains("SlotIcon") && !child.name.Contains("ChangeGuns"))
            {
                gunsWheelSlotsTransform.Add(child.transform as RectTransform);
            }

        }


    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
