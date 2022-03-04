using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

    [SerializeField] Camera mainCamera;

    public KeyCode fireHotKey;

    RaycastHit raycastHit;

    float raycastMaxRange = 5f;

    private void Awake()
    {
        fireHotKey = KeyCode.Mouse0;
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(fireHotKey))
        {
            Hit();
        }

    }

    void Hit()
    {
        if(Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out raycastHit, raycastMaxRange))
        {
            if(raycastHit.transform != null && raycastHit.transform.name != "Floor")
            {
                //Debug.Log(raycastHit.transform.name);

                raycastHit.transform.gameObject.GetComponent<ItemData>().DropItem();
            }
        }
    }

}
