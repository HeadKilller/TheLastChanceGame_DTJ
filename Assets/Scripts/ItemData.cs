using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    [SerializeField] Items item;

    bool isDropped;

    private void Awake()
    {
        isDropped = false;
    }

    private void FixedUpdate()
    {
        if (isDropped)
        {
            Vector3 rotation_while_dropped = new Vector3(0f, 5f, 0f);

            transform.Rotate(rotation_while_dropped);
        }
    }

    public void DropItem()
    {
        float scale = 0.3f;

        transform.localScale = new Vector3(scale, scale, scale);
        isDropped = true;
        GetComponent<BoxCollider>().isTrigger = true;


    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Inventory.instance.AddItem(gameObject, item);
        }
    }

}
