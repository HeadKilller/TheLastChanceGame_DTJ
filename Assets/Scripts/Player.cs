using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject gunPosition;

    Dictionary<GunType, GameObject> equippedGuns;
    PlayerInputControl playerInputControl;

    GameObject selectedGun;

    RaycastHit raycastHit;


    #region Propriedades

    public GameObject SelectedGun
    {
        get { return selectedGun; }
    }

    #endregion

    private void Awake()
    {

        playerInputControl = new PlayerInputControl();

        //equippedGuns.Add(GunType.HandGun, null);
        //equippedGuns.Add(GunType.AssaultRifle, null);

        selectedGun = null;

        playerInputControl.PlayerOnFoot.Enable();

        playerInputControl.PlayerOnFoot.Equip.performed += SelectGun;

        if(selectedGun != null)
        {

            playerInputControl.PlayerOnFoot.FireAuto.performed += FireAuto;
            playerInputControl.PlayerOnFoot.FireSemi.performed += FireSemi;

        }
    }


    public void FireSemi(InputAction.CallbackContext context)
    {

        if(Physics.Raycast(mainCamera.transform.position, 
            mainCamera.transform.forward, 
            out raycastHit, 
            selectedGun.GetComponent<ItemData>().Gun.maxRange))
        {
            Debug.Log("Semi has Hit : " + raycastHit.transform.name);
        }

    }

    public void FireAuto(InputAction.CallbackContext context)
    {

        if (Physics.Raycast(mainCamera.transform.position,
            mainCamera.transform.forward,
            out raycastHit,
            selectedGun.GetComponent<ItemData>().Gun.maxRange))
        {
            Debug.Log("Auto has Hit : " + raycastHit.transform.name);
        }

    }

    public void SelectGun(InputAction.CallbackContext context)
    {

        float range = 20f;

        if (Physics.Raycast(mainCamera.transform.position,
            mainCamera.transform.forward,
            out raycastHit,
            range))
        {

            Guns gun = raycastHit.transform.gameObject.GetComponent<ItemData>().Gun;
           
            if(gun != null)
            {

                selectedGun = Instantiate(raycastHit.transform.gameObject, gunPosition.transform);

                try
                {

                    selectedGun.GetComponent<Rigidbody>().isKinematic = true;
                }
                catch
                {
                    Debug.Log("NULL");
                }

                Destroy(raycastHit.transform.gameObject);


            }

        }

    }



}


