using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Linq;

public class Inventory : MonoBehaviour
{

    [SerializeField] GameObject inventoryGameObject;
    [SerializeField] GameObject inventorySlotsParent;

    public static Inventory instance;

    PlayerInputControl playerInputControl;

    ItemData[] inventory;
    GameObject[] inventorySlots;

    int inventorySize;

    Dictionary<GameObject, Items> inventory_with_Item;

    private void Awake()
    {
        instance = this;

        inventorySize = 20;
        inventory = new ItemData[inventorySize];
        inventorySlots = new GameObject[inventorySize];
        inventory_with_Item = new Dictionary<GameObject, Items>();

        int count = 1;

        foreach (Transform child in inventorySlotsParent.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.name == "InventorySlot" + count.ToString())
            {
                inventory_with_Item.Add(child.gameObject, null);
                count++;
            }
        }

        playerInputControl = new PlayerInputControl();

        playerInputControl.UI.Enable();

        playerInputControl.UI.Inventory.performed += InventoryOpen_Close;
    }

    

    public void AddItem(GameObject _gameObject, Items item)
    {

        bool foundEmptySlot = false;

        foreach (var slot in inventory_with_Item)
        {
            if (slot.Value == null)
            {
                inventory_with_Item[slot.Key] = item;
                foundEmptySlot = true;
                ActivateSlot(slot.Key);
                break;
            }
        }

        if (!foundEmptySlot) return;

        Destroy(_gameObject);
        Debug.Log("Picking up " + item.name);

    }

    public void RemoveItem(GameObject _gameObject)
    {
        inventory_with_Item[_gameObject] = null;
        DeActivateSlot(_gameObject);
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

}
