using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{

    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject gameController;
    [SerializeField] GameObject MissionObjectiveRadio;

    [SerializeField] LayerMask ignoreRaycast;

    PlayerInputControl playerInputControl;


    RaycastHit raycastHit;

    float raycastMaxRange = 10f;

    private void Awake()
    {

        playerInputControl = new PlayerInputControl();

        playerInputControl.PlayerOnFoot.Enable();

        playerInputControl.PlayerOnFoot.Interact.performed += Interact;
    }

    private void Interact(InputAction.CallbackContext context)
    {


        if (Physics.Raycast(mainCamera.transform.position,
            mainCamera.transform.forward,
            out raycastHit,
            raycastMaxRange, ignoreRaycast))
        {

            

            //Debug.Log("It has hit : " + raycastHit.transform.name);

            if (raycastHit.transform.tag == "Gun")
            {
                if(raycastHit.transform.name == "PistolTUTO")
                {
                    raycastHit.transform.GetComponent<GunPicked>().pickGun = true;
                }
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

                if(material.GetComponent<ItemData>().Item.name == "Scrap Metal")
                {
                    System.Random rnd = new System.Random();

                    float numberToAdd = rnd.Next(2, 8);

                    for(int i = 1; i < numberToAdd; i++)
                    {
                        Inventory.instance.AddItem(material, material.GetComponent<ItemData>().Item, material, true);
                    }

                }
                else if (material.GetComponent<ItemData>().Item.name == "Copper Wire")
                {
                    System.Random rnd = new System.Random();

                    float numberToAdd = rnd.Next(1, 4);

                    for (int i = 1; i < numberToAdd; i++)
                    {
                        Inventory.instance.AddItem(material, material.GetComponent<ItemData>().Item, material, true);
                    }

                }
                else
                {
                    Inventory.instance.AddItem(material, material.GetComponent<ItemData>().Item, material, true);
                }


            }

            if(raycastHit.transform.tag == "Munition")
            {
                //Debug.Log("Picking Up Munition");

                Items tempGun = raycastHit.transform.gameObject.GetComponent<ItemData>().Item;

                this.GetComponent<PlayerGun>().PickMunition(tempGun);

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

            if(raycastHit.transform.tag == "Radio")
            {

                MissionObjectiveRadio.GetComponent<Mission3>().callBackUp = true;
              

            }
        }

    }
   

}
