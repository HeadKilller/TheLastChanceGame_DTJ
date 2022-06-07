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


    RaycastHit raycastHit;

    float raycastMaxRange = 5f;

    private void Awake()
    {

        playerInputControl = new PlayerInputControl();

        playerInputControl.PlayerOnFoot.Enable();

        playerInputControl.PlayerOnFoot.Interact.performed += Interact;
    }

    private void Interact(InputAction.CallbackContext context)
    {

        float range = 100f;

        if (Physics.Raycast(mainCamera.transform.position,
            mainCamera.transform.forward,
            out raycastHit,
            range))
        {

            //Debug.Log("It has hit : " + raycastHit.transform.name);

            if (raycastHit.transform.tag == "Gun")
            {
                GameObject equippedGun;

                equippedGun = raycastHit.transform.gameObject;

                //this.GetComponent<PlayerGun>().EquipGun(equippedGun);

                Inventory.instance.AddItem(equippedGun, equippedGun.GetComponent<ItemData>().Gun,equippedGun, false);

                return;
            }

            if (raycastHit.transform.tag == "Crafting Table")
            {
                //Debug.Log("Crafting...");

                gameController.GetComponent<Craft>().OpenCraftingMenu();

            }

            if (raycastHit.transform.tag == "Material")
            {
                //Debug.Log("Picking Up Material");

                GameObject material = raycastHit.transform.gameObject;

                //raycastHit.transform.gameObject.GetComponent<ItemData>().DropItem();

                Inventory.instance.AddItem(material, material.GetComponent<ItemData>().Item, material, true);

            }

            if(raycastHit.transform.tag == "Munition")
            {
                //Debug.Log("Picking Up Munition");

                this.GetComponent<PlayerGun>().PickMunition(raycastHit.transform.name);

                Destroy(raycastHit.transform.gameObject);
            }

            if(raycastHit.transform.tag == "Destructable")
            {

                raycastHit.transform.gameObject.GetComponent<ItemData>().DropItem();

            }

            if (raycastHit.transform.tag == "Trap")
            {

                GameObject trap = raycastHit.transform.gameObject;

                Inventory.instance.AddItem(trap, trap.GetComponent<ItemData>().Item, trap, false);

            }

            //Debug.Log(raycastHit.transform.name);
            if (raycastHit.transform.name == "OutsideButton" || raycastHit.transform.name == "InsideButton")
            {
                raycastHit.transform.GetComponent<ButtonOpenGate>().activated = true;
            }


        }

    }
   

}
