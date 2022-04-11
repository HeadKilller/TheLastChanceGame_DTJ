using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{

    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject gameController;

    PlayerInputControl playerInputControl;

    public KeyCode fireHotKey;

    RaycastHit raycastHit;

    float raycastMaxRange = 5f;

    private void Awake()
    {
        fireHotKey = KeyCode.Mouse0;

        playerInputControl = new PlayerInputControl();

        playerInputControl.PlayerOnFoot.Enable();

        playerInputControl.PlayerOnFoot.Interact.performed += Interact;
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(fireHotKey) && gameObject.GetComponent<PlayerGun>().SelectedGun == null)
        {
            try
            {
                Hit();
            }
            catch(Exception e)
            {
                Debug.Log("Error : " + e);
            }
        }

    }
    private void Interact(InputAction.CallbackContext context)
    {

        float range = 20f;

        if (Physics.Raycast(mainCamera.transform.position,
            mainCamera.transform.forward,
            out raycastHit,
            range))
        {

            if (raycastHit.transform.tag == "Gun")
            {
                GameObject equippedGun;

                equippedGun = raycastHit.transform.gameObject;

                this.GetComponent<PlayerGun>().EquipGun(equippedGun);

                return;
            }

            if (raycastHit.transform.tag == "Crafting Table")
            {
                Debug.Log("Crafting...");

                gameController.GetComponent<Craft>().OpenCraftingMenu();

            }

            if (raycastHit.transform.tag == "Material")
            {
                Debug.Log("Picking Up Material");
            }


        }

    }
    void Hit()
    {
        if(Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out raycastHit, raycastMaxRange))
        {
            if(raycastHit.transform != null && raycastHit.transform.tag != "Environment" && raycastHit.transform.gameObject.layer != 5)
            {
                //Debug.Log(raycastHit.transform.gameObject.GetComponent<ItemData>());

                raycastHit.transform.gameObject.GetComponent<ItemData>().DropItem();
            }
        }
    }

}
