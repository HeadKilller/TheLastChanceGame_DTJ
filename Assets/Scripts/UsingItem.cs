using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsingItem : MonoBehaviour
{

    LayerMask groundLayer;
    LayerMask ignoreLayer;

    GameObject itemToDrag;
    bool isDragging;
    bool canPutItem;

    public static UsingItem instance;

    int indexAtInventory;

    // Start is called before the first frame update
    void Start()
    {
        itemToDrag = null;
        isDragging = false;
        canPutItem = false;

        indexAtInventory = -1;

        instance = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log("Is Dragging : " + isDragging);

        if (isDragging)
        {

            float hitRadius = 0.1f;

            Vector3 tempPosition = GetPosition();
            Vector3 position = new Vector3(tempPosition.x, tempPosition.y + (itemToDrag.transform.position.y - tempPosition.y), tempPosition.z);

            //position.y = 1.1f;

            if (itemToDrag.transform.localScale.x > itemToDrag.transform.localScale.z)
            {
                hitRadius = itemToDrag.transform.localScale.x / 2f;
            }
            else
            {

                hitRadius = itemToDrag.transform.localScale.z / 2f;

            }

            if (!Physics.CheckSphere(position, hitRadius, ignoreLayer))
            {
                Debug.Log("Cant put down");
                itemToDrag.SetActive(false);
                canPutItem = false;
            }
            else
            {
                Debug.Log("Can put down");

                itemToDrag.transform.position = position;
                itemToDrag.SetActive(true);
                canPutItem = true;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                StopDragging();
            }




        }
    }


    public void DragItem(GameObject _itemToDrag, int _indexAtInventory, LayerMask _groundLayer, LayerMask _ignoreLayer)
    {

        groundLayer = _groundLayer;
        ignoreLayer = _ignoreLayer;

        ignoreLayer |= (1 << _itemToDrag.layer);       

        itemToDrag = _itemToDrag;
        isDragging = true;
        indexAtInventory = _indexAtInventory;

    }

    public void StopDragging()
    {        

        if (canPutItem)
        {

            Debug.Log("Put item on ground");

            Debug.Log("Index : " + indexAtInventory);
            GameObject slot = Inventory.instance.Inventory_Slots[indexAtInventory];

            Inventory.instance.RemoveItem(itemToDrag.GetComponent<ItemData>().Item, 1);
            canPutItem = false;
        }
        else
        {
            Debug.Log("Do not put item on ground");

            itemToDrag.SetActive(false);
        }

        itemToDrag = null;
        isDragging = false;

    }
    private Vector3 GetPosition()
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

}
