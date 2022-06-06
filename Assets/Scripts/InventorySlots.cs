using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InventorySlots : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject ItemUsagePanel;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] LayerMask groundLayer;

    [SerializeField] Button UseButton, CloseButton;


    [SerializeField] GameObject trapsParent;

    Items item;

    GameObject itemGameObject;


    private void Start()
    {
        itemGameObject = null;

       
    }

    private void Awake()
    {

        UseButton.onClick.AddListener(UseItem);
        CloseButton.onClick.AddListener(ClosePanel);

    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {

            //Debug.Log("Clicked");

            Vector3 offset = new Vector3(-20f, -10f, 0f);

            ItemUsagePanel.transform.position = Input.mousePosition + offset;


            if(FindIndex() != -1)
            {

                item = Inventory.instance.Inventory_SlotsContent[FindIndex()];

                

                if (item.isUsable)
                {
                    ItemUsagePanel.SetActive(true);
                    //Debug.Log("It's Usable");
                }

                else
                {
                    //Debug.Log("Not Usable");
                    item = null;
                    ClosePanel();
                }

            }


        }
        else
        {
            //Debug.Log("Not clicking");
        }

        

    }



    private int FindIndex()
    {

        string[] splited = this.name.Split(' ');
        int index = -1;

        try
        {
            index = Int32.Parse(splited[1]);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }

        if (index != -1)
            index -= 1;

        return index;

    }

    private Vector3 GetPosition()
    {

        Vector3 position = Vector3.zero;

        float maxDistance = 10f;
        RaycastHit hit;

        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance, groundLayer))
        {

            Debug.Log("Hit Ground");
            position = hit.point;

        }
        else
        {
            Debug.Log("Doesnt hit ground");
        }

        return position;
    }
    
    public void ClosePanel()
    {

        ItemUsagePanel.SetActive(false);

    }
    
    public void UseItem()
    {



        if (item == null)
            return;

        if (item.itemType == ItemType.Trap)
        {

            Inventory.instance.Close_Inventory();
            ClosePanel();

            int index = FindIndex();

            if(index == -1)
            {
                Debug.Log("Error. Index not found.");
                return;
            }
            
            itemGameObject = Inventory.instance.inventorySlotsContent_Objects[index];

            if (itemGameObject == null)
            {
                Debug.Log("Error. There's no game object on that inventory slot.");
                return;
            }


            itemGameObject.transform.parent = trapsParent.transform;

            itemGameObject.SetActive(true);

            if (GetPosition() == Vector3.zero)
                itemGameObject.SetActive(false);

            UsingItem.instance.DragItem(itemGameObject, index, groundLayer, ignoreLayer);

        }

        if(item.itemType == ItemType.Gun)
        {
            Inventory.instance.Close_Inventory();
            ClosePanel();

            int index = FindIndex();

            if (index == -1)
            {
                Debug.Log("Error. Index not found.");
                return;
            }

            itemGameObject = Inventory.instance.inventorySlotsContent_Objects[index];

            if (itemGameObject == null)
            {
                Debug.Log("Error. There's no game object on that inventory slot.");
                return;
            }

            PlayerGun.instance.EquipGun(itemGameObject);

            Inventory.instance.RemoveItem(itemGameObject.GetComponent<ItemData>().Gun, 1);

        }
    }

    

}
