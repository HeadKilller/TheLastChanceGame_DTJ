using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InventorySlots : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject ItemUsagePanel;
    
    Items item;


    int currentIndex;


    UseItem useItemScript;

    private void Start()
    {

        currentIndex = -1;

        useItemScript = Player.instance.gameObject.GetComponent<UseItem>();

    }

    private void Awake()
    {

       

    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {

            //Debug.Log("Clicked");

            Vector3 offset = new Vector3(-20f, -10f, 0f);

            ItemUsagePanel.transform.position = Input.mousePosition + offset;

            currentIndex = FindIndex();

            //Debug.Log("Index  : " + currentIndex);

            if(currentIndex != -1)
            {
                item = Inventory.instance.Inventory_SlotsContent[currentIndex];

                //Debug.Log("Item : " + item.name);

                if (item.isUsable)
                {
                    ItemUsagePanel.SetActive(true);
                    useItemScript.Index = currentIndex;
                }

                else
                {
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

        string[] splited = gameObject.name.Split(' ');
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

   
    public void ClosePanel()
    {

        ItemUsagePanel.SetActive(false);

    }
    
    

    

}
