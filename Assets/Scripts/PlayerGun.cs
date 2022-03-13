using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class PlayerGun : MonoBehaviour
{

    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject gunPosition;
    [SerializeField] GameObject bulletHolePrefab;

    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] ParticleSystem hitParticleSystem;

    [SerializeField] GameObject currentBulletsPanel;
    [SerializeField] GameObject changeGunPanel;

    [SerializeField] GameObject handGun_Slot;
    [SerializeField] GameObject assaultGun_Slot;

    Dictionary<GameObject, GameObject> equippedGuns;

    

    PlayerInputControl playerInputControl;

    GameObject selectedGun;

    TMP_Text currentBulletsText;
    Image bulletsIcon;

    RaycastHit raycastHit;

    float autoTimer;
    float fireRate;
    float magCapacity;

    float currentBullets, currentMags;

    #region Propriedades

    public GameObject SelectedGun
    {
        get { return selectedGun; }
    }

    #endregion


    private void Start()
    {
        currentBulletsText = currentBulletsPanel.GetComponentInChildren<TMP_Text>();
        bulletsIcon = currentBulletsPanel.GetComponentInChildren<Image>();

        currentBulletsText.color = new Color(1, 1, 1);

        selectedGun = null;
        autoTimer = 0;
        fireRate = 0;
        magCapacity = 0;
        currentBullets = 0;
        currentMags = 0;

    }

    private void Awake()
    {

        playerInputControl = new PlayerInputControl();

        equippedGuns = new Dictionary<GameObject, GameObject>();

        equippedGuns.Add(handGun_Slot, null);
        equippedGuns.Add(assaultGun_Slot, null);


        playerInputControl.PlayerOnFoot.Enable();

        playerInputControl.PlayerOnFoot.Equip.performed += EquipGun;

        playerInputControl.PlayerOnFoot.ReloadGun.performed += ReloadGun;

        playerInputControl.PlayerOnFoot.ChangeGun.performed += ChangeGun;
        playerInputControl.PlayerOnFoot.ChangeGun.canceled += GunSelected;

        playerInputControl.PlayerOnFoot.FireSemi.performed += FireSemi;

        

        
    }

    private void Update()
    {

        if(selectedGun != null) currentBulletsText.text = currentBulletsText.text = string.Format("{0} / {1}", currentBullets, currentMags);


        if (selectedGun != null && playerInputControl.PlayerOnFoot.FireAuto.ReadValue<float>() == 1)
        {
            FireAuto();
        }

    }


    public void FireSemi(InputAction.CallbackContext context)
    {
       
        if (selectedGun != null && currentBullets > 0)
        {

            muzzleFlash.Play();
            currentBullets--;

            if (Physics.Raycast(mainCamera.transform.position, 
                mainCamera.transform.forward, 
                out raycastHit, 
                selectedGun.GetComponent<ItemData>().Gun.maxRange))
            {
                Debug.Log("Semi has Hit : " + raycastHit.transform.name);

                if(raycastHit.transform.gameObject != bulletHolePrefab)
                {
                    
                        Instantiate(bulletHolePrefab, 
                            raycastHit.point + raycastHit.normal * 0.001f, 
                            Quaternion.LookRotation(raycastHit.normal, mainCamera.transform.up));

                }


            }

        }


    }

    public void FireAuto()
    {


        if (currentBullets > 0 && autoTimer >= (1f / fireRate) ){

            muzzleFlash.Play();
            currentBullets--;

            if (Physics.Raycast(mainCamera.transform.position,      
            mainCamera.transform.forward,
            out raycastHit,
            selectedGun.GetComponent<ItemData>().Gun.maxRange))
            {
                Debug.Log("Auto has Hit : " + raycastHit.transform.name);

                if (raycastHit.transform.gameObject != bulletHolePrefab)
                {

                    Instantiate(bulletHolePrefab,
                        raycastHit.point + raycastHit.normal * 0.001f,
                        Quaternion.LookRotation(raycastHit.normal, mainCamera.transform.up));

                }
            }

            autoTimer = 0f;

            //Debug.Log(autoTimer);

        }

        autoTimer += Time.deltaTime;

    }

    public void GunSelected(InputAction.CallbackContext context)
    {


        if (selectedGun != null)
        {
            selectedGun.SetActive(true);

            switch (selectedGun.GetComponent<ItemData>().Gun.gunType)
            {
                case GunType.HandGun:

                    handGun_Slot.GetComponent<Button>().interactable = false;
                    break;
                case GunType.AssaultRifle:

                    assaultGun_Slot.GetComponent<Button>().interactable = false;
                    break;

            }
            
            currentBulletsText.text = currentBullets.ToString() + " / " + currentMags.ToString();

            currentBulletsPanel.SetActive(true);


        }

            changeGunPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    public void ReloadGun(InputAction.CallbackContext context)
    {

        if(currentMags > 0)
        {

            currentBullets = magCapacity;
            currentMags--;

        }

    }

    public void EquipGun(InputAction.CallbackContext context)
    {

        float range = 20f;

        if (Physics.Raycast(mainCamera.transform.position,
            mainCamera.transform.forward,
            out raycastHit,
            range))
        {

            GameObject equippedGun;

            equippedGun = raycastHit.transform.gameObject;

            if (equippedGun != null)
            {

                equippedGun.transform.SetParent(gunPosition.transform);

                equippedGun.transform.localPosition = Vector3.zero;
                equippedGun.transform.localRotation = Quaternion.identity;

                equippedGun.GetComponent<Rigidbody>().isKinematic = true;
                equippedGun.GetComponentInChildren<Collider>().isTrigger = true;

                equippedGun.SetActive(false);

                switch (equippedGun.GetComponent<ItemData>().Gun.gunType)
                {
                    case GunType.HandGun:

                        equippedGuns[handGun_Slot] = equippedGun;
                        handGun_Slot.GetComponent<Button>().interactable = true;
                        break;
                    case GunType.AssaultRifle:

                        equippedGuns[assaultGun_Slot] = equippedGun;
                        assaultGun_Slot.GetComponent<Button>().interactable = true;
                        break;
                }

            }
        }

    }

    public void ChangeGun(InputAction.CallbackContext context)
    {

        changeGunPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

    }

    public void SelectGun(Button button)
    {

        if (equippedGuns[button.gameObject] == selectedGun) return;

        if(selectedGun != null)
        {
            selectedGun.gameObject.SetActive(false);

            switch (selectedGun.GetComponent<ItemData>().Gun.gunType)
            {
                case GunType.HandGun:

                    handGun_Slot.GetComponent<Button>().interactable = true;
                    break;
                case GunType.AssaultRifle:

                    assaultGun_Slot.GetComponent<Button>().interactable = true;
                    break;

            }

            SaveGunInfo(selectedGun);
        }

        Debug.Log(selectedGun);

        selectedGun = equippedGuns[button.gameObject];

        if(selectedGun != null)
        {
            fireRate = selectedGun.GetComponent<ItemData>().Gun.rateOfFire / 60f;
            magCapacity = selectedGun.GetComponent<ItemData>().Gun.magCapacity;

            currentBullets = magCapacity;
            currentMags = selectedGun.GetComponent<ItemData>().Gun.init_MagNum;
        }

        Debug.Log(selectedGun);

    }
   
    void SaveGunInfo(GameObject gunGameObject)
    {

    }

    void LoadGunInfo(GameObject gunGameObject)
    {

    }

}


