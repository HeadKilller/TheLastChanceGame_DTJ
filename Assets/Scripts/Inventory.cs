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

    Dictionary<GameObject, Items> inventory_with_Item;
    Dictionary<GameObject, int> inventorySlotCurrentCapacity;

    private void Awake()
    {
        #region Variables Initialization

        instance = this;


        inventory_with_Item = new Dictionary<GameObject, Items>();
        inventorySlotCurrentCapacity = new Dictionary<GameObject, int>();

        #endregion

        #region Variables First Values

        gameObjectToDestroy = null;
        slotMaxCapacity = 99;

        int count = 1;



        foreach (Transform child in inventorySlotsParent.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.name == "InventorySlot" + count.ToString())
            {
                inventory_with_Item.Add(child.gameObject, null);
                inventorySlotCurrentCapacity.Add(child.gameObject, 0);
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
        //int count = 0;

        //foreach(var slot in inventory_with_Item)
        //{

        //    Debug.Log("Slot " + count + " : " + slot.Value);
        //    count++;

        //}
    }

    #region Inventory

    public void AddItem(GameObject _gameObject, Items item, bool toDestroy)
    {

        bool foundEmptySlot = false;

        if (item.isStackable)
        {

            foreach(var slot in inventory_with_Item)
            {
                if (slot.Value == null) continue;

                if(slot.Value.name == item.name && inventorySlotCurrentCapacity[slot.Key] < slotMaxCapacity)
                {
                    inventorySlotCurrentCapacity[slot.Key]++;
                    TextMeshProUGUI text = slot.Key.GetComponentInChildren<TextMeshProUGUI>();
                    if(text != null)
                        text.SetText(inventorySlotCurrentCapacity[slot.Key].ToString());

                    foundEmptySlot = true;

                }
            }

        }

        if (!foundEmptySlot)
        {
            foreach (var slot in inventory_with_Item)
            {           

                if (slot.Value == null)
                {
                    //if (item == null)
                    //    Debug.Log("item is null");
                    //Debug.Log("Item : " + item.name);

                    inventory_with_Item[slot.Key] = item;

                    //Debug.Log("Item in inventory : " + inventory_with_Item[slot.Key].name);

                    inventorySlotCurrentCapacity[slot.Key]++;
                    TextMeshProUGUI text = slot.Key.GetComponentInChildren<TextMeshProUGUI>();
                    if(text != null)
                        text.SetText(inventorySlotCurrentCapacity[slot.Key].ToString());

                    foundEmptySlot = true;
                    ActivateSlot(slot.Key);
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


        confirmationWindow_GameObject.SetActive(true);

        confirmationWindow_Slider.maxValue = inventorySlotCurrentCapacity[_gameObject];

        gameObjectToDestroy = _gameObject;

        //inventory_with_Item[_gameObject] = null;
        //inventorySlotCurrentCapacity[_gameObject] = 0;

        //DeActivateSlot(_gameObject);

        //TextMeshProUGUI text = _gameObject.GetComponentInChildren<TextMeshProUGUI>();
        //if (text != null && inventorySlotCurrentCapacity[_gameObject] != 0)
        //    text.SetText(inventorySlotCurrentCapacity[_gameObject].ToString());
        //if (text != null && inventorySlotCurrentCapacity[_gameObject] == 0)
        //    text.SetText("");
    }

    public void RemoveItem(Items item, int quantityToRemove)
    {

        GameObject itemToRemove = null;

        foreach (var slot in inventory_with_Item)
        {
            //if (slot.Value == null)
            //    Debug.Log("Slot is Null");
            //Debug.Log("Slot : " + slot.Value.name);
            //Debug.Log("Item : " + item.name);

            if (slot.Value == item)
            {
                //Debug.Log("Item : " + item.name);
                itemToRemove = slot.Key;

            }

            if (itemToRemove == null) return;

        }

        TextMeshProUGUI text = itemToRemove.GetComponentInChildren<TextMeshProUGUI>();
        inventorySlotCurrentCapacity[itemToRemove] -= quantityToRemove;

        Debug.Log("Item : " + inventory_with_Item[itemToRemove]);
        Debug.Log("Quantity : " + inventorySlotCurrentCapacity[itemToRemove]);

        if(inventorySlotCurrentCapacity[itemToRemove] <= 0)
        {
            inventorySlotCurrentCapacity[itemToRemove] = 0;
            text.SetText("");
            DeActivateSlot(itemToRemove);
        }
        else
        {
            text.SetText(inventorySlotCurrentCapacity[itemToRemove].ToString());
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

    private void ActivateSlot(GameObject _gameObject)
    {
        foreach (Transform child in _gameObject.GetComponentsInChildren<Transform>())
        {
            if (child.name == "SlotButton")
            {
                child.GetComponent<Image>().sprite = inventory_with_Item[_gameObject].icon;
                child.GetComponent<Image>().enabled = true;
                child.GetComponent<Button>().enabled = true;
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
        Debug.Log("Destroying... : " + inventory_with_Item[_gameObject]);
        inventory_with_Item[_gameObject] = null;

        foreach (Transform child in _gameObject.GetComponentsInChildren<Transform>())
        {
            if (child.name == "SlotButton")
            {
                child.GetComponent<Image>().sprite = null;
                child.GetComponent<Image>().enabled = false;
                child.GetComponent<Button>().enabled = false;
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


        if (confirmationWindow_Slider.value <= inventorySlotCurrentCapacity[gameObjectToDestroy])
            inventorySlotCurrentCapacity[gameObjectToDestroy] -= (int)confirmationWindow_Slider.value;
        else
            inventorySlotCurrentCapacity[gameObjectToDestroy] = 0;


        TextMeshProUGUI text = gameObjectToDestroy.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null && inventorySlotCurrentCapacity[gameObjectToDestroy] != 0)
            text.SetText(inventorySlotCurrentCapacity[gameObjectToDestroy].ToString());
        if (text != null && inventorySlotCurrentCapacity[gameObjectToDestroy] == 0)
            text.SetText("");

        if(inventorySlotCurrentCapacity[gameObjectToDestroy] == 0)
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

        foreach(var slot in inventory_with_Item)
        {

            if(slot.Value != null && slot.Value == material)
            {

                string materialQuantity_String = slot.Key.GetComponentInChildren<TextMeshProUGUI>().text;

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

    #region Equipment



    #endregion


}
