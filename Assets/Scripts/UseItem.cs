using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseItem : MonoBehaviour
{

    [SerializeField] GameObject ItemUsagePanel;
    [SerializeField] Button UseButton, CloseButton;
    [SerializeField] GameObject trapsParent;

    [SerializeField] LayerMask groundLayer, ignoreLayer;

    int index;


    public int Index
    {

        get { return index; }
        set { index = value; }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {

        UseButton.onClick.AddListener(Use_Item);
        CloseButton.onClick.AddListener(ClosePanel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClosePanel()
    {
        ItemUsagePanel.SetActive(false);
    }

    public Vector3 GetPosition()
    {

        Vector3 position = Vector3.zero;

        float maxDistance = 10f;
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance, groundLayer))
        {

            //Debug.Log("Hit Ground");
            position = hit.point;

        }
        else
        {
            //Debug.Log("Doesnt hit ground");
        }

        return position;
    }

    public void Use_Item()
    {

        Debug.Log("Index : " + index);

        Items currentItem = Inventory.instance.Inventory_SlotsContent[index];
        GameObject currentObject = Inventory.instance.Inventory_SLotsContent_Objects[index];


        if (currentItem == null)
            return;

        if (currentItem.itemType == ItemType.Munitions)
        {

            //Debug.Log("Consumibles");

            //Inventory.instance.Close_Inventory();
            ClosePanel();

            if (index == -1)
            {
                Debug.Log("Error. Index not found.");
                return;
            }

            currentObject = Inventory.instance.inventorySlotsContent_Objects[index];

            if (currentObject == null)
            {
                Debug.Log("Error. There's no game object on that inventory slot.");
                return;
            }

            Items tempItem = Inventory.instance.Inventory_SlotsContent[index];

            PlayerGun.instance.PickMunition(tempItem);

            //Debug.Log("Removing Item now");

            Inventory.instance.RemoveItem(tempItem, 1);

        }

        else if (currentItem.itemType == ItemType.Gun)
        {
            //Inventory.instance.Close_Inventory();
            ClosePanel();

            if (index == -1)
            {
                Debug.Log("Error. Index not found.");
                return;
            }

            currentObject = Inventory.instance.inventorySlotsContent_Objects[index];

            if (currentObject == null)
            {
                Debug.Log("Error. There's no game object on that inventory slot.");
                return;
            }


            PlayerGun.instance.EquipGun(currentObject);

            Inventory.instance.RemoveItem(currentObject.GetComponent<ItemData>().Gun, 1);

        }

        else if (currentItem.itemType == ItemType.Consumible)
        {

            ClosePanel();

            if (index == -1)
            {
                Debug.Log("Error. Index not found.");
                return;
            }

            //itemGameObject = Inventory.instance.inventorySlotsContent_Objects[index];

            //if (itemGameObject == null)
            //{
            //    Debug.Log("Error. There's no game object on that inventory slot.");
            //    return;
            //}


            Player.instance.UseConsumible(currentItem);


            Inventory.instance.RemoveItem(currentItem, 1);

        }

        else if (currentItem.itemType == ItemType.Trap)
        {

            Inventory.instance.Close_Inventory();
            ClosePanel();

            if (index == -1)
            {
                Debug.Log("Error. Index not found.");
                return;
            }


            if (currentObject == null)
            {
                Debug.Log("Error. There's no game object on that inventory slot.");
                return;
            }


            currentObject.transform.parent = trapsParent.transform;

            currentObject.SetActive(true);

            if (GetPosition() == Vector3.zero)
                currentObject.SetActive(false);

            PlaceItem.instance.DragItem(currentObject, index, groundLayer, ignoreLayer);

        }

        else if (currentItem.itemType == ItemType.Radio)
        {

            Inventory.instance.Close_Inventory();
            ClosePanel();

            if (index == -1)
            {
                Debug.Log("Error. Index not found.");
                return;
            }

            currentObject = Inventory.instance.inventorySlotsContent_Objects[index];

            if (currentObject == null)
            {
                Debug.Log("Error. There's no game object on that inventory slot.");
                return;
            }

            currentObject.SetActive(true);

            if (GetPosition() == Vector3.zero)
                currentObject.SetActive(false);

            PlaceItem.instance.DragItem(currentObject, index, groundLayer, ignoreLayer);

            Inventory.instance.RemoveItem(currentObject.GetComponent<ItemData>().Item, 1);

        }



    }

}
