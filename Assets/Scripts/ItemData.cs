using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    [SerializeField] Items item;
    [SerializeField] Guns gun;

    bool isDropped;

    [SerializeField] List<string> droppableItems_Names;
    [SerializeField] List<GameObject> droppableItems_Objects;

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


        System.Random rnd = new System.Random();

        int nDrops = rnd.Next(1, 4);

        int index = droppableItems_Names.IndexOf(item.itemDropped);

        Vector3 dropPosition = gameObject.transform.position + transform.up * 1.5f;

        for(int i = 0; i < nDrops; i++)
        {
            Instantiate(droppableItems_Objects[index], dropPosition, Quaternion.identity);
        }

        Destroy(gameObject);





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

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (this.item != null && other.tag == "Player")
    //    {
    //        Inventory.instance.AddItem(gameObject, item, true);
    //    }
    //}

}
