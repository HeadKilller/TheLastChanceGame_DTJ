using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    [SerializeField] Items item;
    [SerializeField] Guns gun;
    [SerializeField] GameObject playerGameObject;

    bool isDropped;


    public Items Item
    {
        get { return item; }
    }
    public Guns Gun
    {
        get { return gun; }
    }

    private void Awake()
    {
        isDropped = false;
        
    }

    private void FixedUpdate()
    {
        if (isDropped)
        {
            Vector3 rotation_while_dropped = new Vector3(0f, 5f, 0f);

            transform.Rotate(rotation_while_dropped, Space.World);
        }
    }

    public void DropItem()
    {

        //TODO: Passar para RigidBody e arranjar maneira de nao passar pelo chao quando cai.

        

        float scale = 0.3f;

        transform.localScale = new Vector3(scale, scale, scale);

        //transform.position = playerGameObject.transform.position + playerGameObject.transform.forward * 2;

        transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);

        isDropped = true;

        try
        {

            GetComponent<Collider>().isTrigger = true;

        }
        catch
        {
            GetComponentInChildren<Collider>().isTrigger = true;
        }


    }

    public Dictionary<List<string>, Sprite> ShowItemInfo()
    {
        Dictionary<List<string>, Sprite > tempDict = new Dictionary<List<string>, Sprite>();

        List<string> tempDataToShow = new List<string>();

        Sprite itemIcon = item.icon;

        tempDataToShow.Add(item.name);
        tempDataToShow.Add(item.description);
        
        tempDict.Add(tempDataToShow, itemIcon);


        return tempDict;
    }
    public void ShowItemToCraftInfo()
    {

        List<string> tempDataToShow = new List<string>();

        Sprite itemIcon = item.icon;

        tempDataToShow.Add(item.name);
        tempDataToShow.Add(item.description);


    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (this.item != null && other.tag == "Player")
        {
            Inventory.instance.AddItem(gameObject, item, true);
        }
    }

}
