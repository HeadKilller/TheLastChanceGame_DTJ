using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;


public class Inventory : MonoBehaviour
{

    [SerializeField] GameObject inventoryGameObject;
    [SerializeField] GameObject inventorySlotsParent;


    [SerializeField] GameObject confirmationWindow_GameObject;
 
    [SerializeField] Slider confirmationWindow_Slider;

    
    [SerializeField] int inventorySize;

    public static Inventory instance;

    PlayerInputControl playerInputControl;
       

    int slotMaxCapacity;

    GameObject gameObjectToDestroy;

    

    List<GameObject> inventory_Slots;
    List<Items> inventorySlotsContent;
    List<int> inventorySlotsCurrentCapacity;

    
    private void Awake()
    {
        #region Variables Initialization

        instance = this;

        inventory_Slots = new List<GameObject>();
        inventorySlotsContent = new List<Items>();
        inventorySlotsCurrentCapacity = new List<int>();

        

        #endregion

        #region Variables First Values

        gameObjectToDestroy = null;
        slotMaxCapacity = 99;

        int count = 1;



        foreach (Transform child in inventorySlotsParent.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.name == "InventorySlot" + count.ToString())
            {

                inventory_Slots.Add(child.gameObject);
                inventorySlotsContent.Add(null);
                inventorySlotsCurrentCapacity.Add(0);

                
                count++;
            }
        }

        #endregion

        #region Player Input

        playerInputControl = new PlayerInputControl();

        playerInputControl.UI.Enable();

        playerInputControl.UI.Inventory.performed += InventoryOpen_Close;

        #endregion

        #region Button Listeners

        confirmationWindow_Slider.onValueChanged.AddListener(delegate { ChangeSliderValue(); });

        #endregion
    }


    private void Update()
    {

        if (inventoryGameObject.activeSelf)
        {

            CheckIfIsOverUI();

        }

    }




    #region Inventory

    public void AddItem(GameObject _gameObject, Items item, bool toDestroy)
    {

        bool foundEmptySlot = false;

        if (item.isStackable)
        {

            for(int i = 0; i < inventory_Slots.Count; i++)
            {

                if (inventorySlotsContent[i] == null) continue;

                if (inventorySlotsContent[i] == item && inventorySlotsCurrentCapacity[i] < slotMaxCapacity)
                {

                    inventorySlotsCurrentCapacity[i]++;
                    TextMeshProUGUI text = inventory_Slots[i].GetComponentInChildren<TextMeshProUGUI>();
                    if (text != null)
                        text.SetText(inventorySlotsCurrentCapacity[i].ToString());

                    foundEmptySlot = true;
                }

            }
                                    

        }

        if (!foundEmptySlot)
        {

            for(int i = 0; i < inventory_Slots.Count; i++)
            {

                if(inventorySlotsContent[i] == null)
                {

                    inventorySlotsContent[i] = item;

                    inventorySlotsCurrentCapacity[i]++;

                    TextMeshProUGUI text = inventory_Slots[i].GetComponentInChildren<TextMeshProUGUI>();
                    if (text != null)
                        text.SetText(inventorySlotsCurrentCapacity[i].ToString());

                    foundEmptySlot=true;
                    //Debug.Log(inventory_Slots[i].name);
                    ActivateSlot(inventory_Slots[i], i);
                    break;
                }

            }
                        

        }

        if (!foundEmptySlot) return;

        if(toDestroy)
            Destroy(_gameObject);
        
        //Debug.Log("Picking up " + item.name);

    }
    

    public void RemoveItem(GameObject _gameObject)
    {


        #region TO RESOLVE

        /*if (inventorySlotCurrentCapacity[_gameObject] > 1)
        {
            int numberToDestroy = N_ofItemstoDestroyWindow(_gameObject);


            inventorySlotCurrentCapacity[_gameObject] -= numberToDestroy;
        }
        else
        {
            inventory_with_Item[_gameObject] = null;
            inventorySlotCurrentCapacity[_gameObject] = 0;

            DeActivateSlot(_gameObject);

        }
        */
        #endregion

        int index = -1;

        for(int i = 0; i < inventory_Slots.Count; i++)
        {

            if (inventory_Slots[i] == _gameObject)
                index = i;
        }

        confirmationWindow_GameObject.SetActive(true);

        confirmationWindow_Slider.maxValue = inventorySlotsCurrentCapacity[index];

        gameObjectToDestroy = _gameObject;

        
    }

    public void RemoveItem(Items item, int quantityToRemove)
    {

        GameObject itemToRemove = null;
        int index = -1;
        for(int i = 0; i < inventory_Slots.Count; i++)
        {

            if (inventorySlotsContent[i] == null) continue;

            if(inventorySlotsContent[i] == item)
            {
                itemToRemove = inventory_Slots[i];
                index = i;
                break;
            }
            if (itemToRemove == null) return;

        }
                

        TextMeshProUGUI text = itemToRemove.GetComponentInChildren<TextMeshProUGUI>();
        inventorySlotsCurrentCapacity[index] -= quantityToRemove;

        Debug.Log("Item : " + inventory_Slots[index]);
        Debug.Log("Quantity : " + inventorySlotsCurrentCapacity[index]);

        if(inventorySlotsCurrentCapacity[index] <= 0)
        {
            inventorySlotsCurrentCapacity[index] = 0;
            text.SetText("");
            DeActivateSlot(itemToRemove);
        }
        else
        {
            text.SetText(inventorySlotsCurrentCapacity[index].ToString());
        }

    }

    public void InventoryOpen_Close(InputAction.CallbackContext context)
    {
        inventoryGameObject.SetActive(!inventoryGameObject.activeSelf);

        if (inventoryGameObject.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

        }


    }

    public void Close_Inventory()
    {
        inventoryGameObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void ActivateSlot(GameObject _gameObject, int slot)
    {
        

        foreach (Transform child in _gameObject.GetComponentsInChildren<Transform>())
        {
            Debug.Log(child.name);
            if (child.name == "SlotButton")
            {                  
                child.GetComponent<Button>().enabled = true;
            }

            if(child.name == "SlotImage")
            {
                child.GetComponent<Image>().sprite = inventorySlotsContent[slot].icon;
                child.GetComponent<Image>().enabled = true;
                child.GetComponent<ItemDragHandler>().enabled = true;
            }

            if (child.name == "EmptySlotButton")
            {
                child.GetComponent<Image>().enabled = true;
                child.GetComponent<Button>().enabled = true;
            }

        }
    }

    private void DeActivateSlot(GameObject _gameObject)
    {
        int index = -1;

        for(int i = 0; i < inventory_Slots.Count; i++)
        {

            if (inventory_Slots[i].gameObject == _gameObject)
                index = i;

        }

        Debug.Log("Destroying... : " + inventory_Slots[index]);
        inventorySlotsContent[index] = null;

        foreach (Transform child in _gameObject.GetComponentsInChildren<Transform>())
        {
            if (child.name == "SlotButton")
            {                
                child.GetComponent<Button>().enabled = false;
            }
            if (child.name == "SlotImage")
            {
                child.GetComponent<Image>().sprite = null;
                child.GetComponent<Image>().enabled = true;
                child.GetComponent<ItemDragHandler>().enabled = true;
            }
            if (child.name == "EmptySlotButton")
            {
                child.GetComponent<Image>().enabled = false;
                child.GetComponent<Button>().enabled = false;
            }
        }
    }

    public void DestroyItem()
    {

        int index = -1;
        for ( int i = 0; i < inventory_Slots.Count; i++)
        {
            if (inventory_Slots[i] == gameObjectToDestroy)
                index = i;
        }

        if (confirmationWindow_Slider.value <= inventorySlotsCurrentCapacity[index])
            inventorySlotsCurrentCapacity[index] -= (int)confirmationWindow_Slider.value;
        else
            inventorySlotsCurrentCapacity[index] = 0;


        TextMeshProUGUI text = gameObjectToDestroy.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null && inventorySlotsCurrentCapacity[index] != 0)
            text.SetText(inventorySlotsCurrentCapacity[index].ToString());
        if (text != null && inventorySlotsCurrentCapacity[index] == 0)
            text.SetText("");

        if(inventorySlotsCurrentCapacity[index] == 0)
        {
            DeActivateSlot(gameObjectToDestroy);
        }

        confirmationWindow_Slider.value = 0;

        confirmationWindow_GameObject.SetActive(false);

    }

    public void ChangeSliderValue()
    {

        TMP_Text textTMP = null;

        foreach (var child in confirmationWindow_GameObject.GetComponentsInChildren<TMP_Text>()){
            if (child.name == "SliderValue")
                textTMP = child;

        }

        int value = (int)confirmationWindow_Slider.value;

        Debug.Log(value);

        if (textTMP != null)
            textTMP.text = value.ToString();
    }

    public bool CheckIfHasInInventory(Items material, int materialNum)
    {

        bool canCraft = false;

        for(int i = 0; i < inventory_Slots.Count; i++)
        {

            if(inventorySlotsContent[i] != null && inventorySlotsContent[i] == material)
            {

                string materialQuantity_String = inventory_Slots[i].GetComponentInChildren<TextMeshProUGUI>().text;

                int materialQuantity_Integer = Int32.Parse(materialQuantity_String);

                if (materialQuantity_Integer >= materialNum)
                {
                    canCraft = true;
                }
            }

        }
                

        return canCraft;

    }

    #endregion

    void CheckIfIsOverUI()
    {

        RaycastHit hit;

        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Debug.Log("mouse position : " + mouseRay.origin);


        if (Physics.Raycast(mouseRay, out hit) && hit.collider.tag == "UI")
        {
            //Debug.Log("Is over : " + hit.collider.gameObject.name);

            for(int i = 0; i < inventory_Slots.Count; i++)
            {

                if(hit.collider.gameObject == inventory_Slots[i] && inventorySlotsContent[i] != null)
                {



                }

            }

            

        }

    }

    public Items CheckItemOnHover(GameObject inventorySlot)
    {

        Items itemOnHover = null;

        for(int i = 0; i < inventorySize; i++)
        {

            if(inventory_Slots[i] == inventorySlot)
            {

                itemOnHover = inventorySlotsContent[i];

            }

        }

        return itemOnHover;

    }

    

}
