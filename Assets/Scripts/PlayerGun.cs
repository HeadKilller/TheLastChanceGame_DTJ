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
    Dictionary<GunType, int> bulletsNumber;

    ParticleSystem selectedWeapon_MuzzleFlash;


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
        bulletsNumber = new Dictionary<GunType, int>();

        currentBulletsText = currentBulletsPanel.GetComponentInChildren<TMP_Text>();
        bulletsIcon = currentBulletsPanel.GetComponentInChildren<Image>();

        bulletsNumber.Add(GunType.HandGun, 14);
        bulletsNumber.Add(GunType.AssaultRifle, 60);

        currentBulletsText.color = new Color(1, 1, 1);

        selectedWeapon_MuzzleFlash = null;
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

    //Método chamado quando o jogador carrega uma vez no rato
    public void FireSemi(InputAction.CallbackContext context)
    {

        if (selectedGun != null && currentBullets > 0)
        {

            selectedWeapon_MuzzleFlash.Play();
            currentBullets--;

            Guns currentGun_Info = selectedGun.GetComponent<ItemData>().Gun;

            int dmg = currentGun_Info.damage;

            if (Physics.Raycast(mainCamera.transform.position,
                mainCamera.transform.forward,
                out raycastHit,
                currentGun_Info.maxRange))
            {
                //Debug.Log("Semi has Hit : " + raycastHit.transform.name);

                if (raycastHit.transform.gameObject != bulletHolePrefab && 
                    raycastHit.transform.name != "Player" && 
                    raycastHit.transform.tag != "Crafting Table" && 
                    raycastHit.transform.tag != "Zombie")
                {

                    Instantiate(bulletHolePrefab,
                        raycastHit.point + raycastHit.normal * 0.001f,
                        Quaternion.LookRotation(raycastHit.normal, mainCamera.transform.up));

                }

                if(raycastHit.transform.tag == "Zombie")
                {
                    raycastHit.transform.gameObject.GetComponent<ZombieBehavior>().ZombieHit(dmg);
                }


            }

        }


    }

    //Método chamado quando o jogador mantém o botão do rato pressionado
    public void FireAuto()
    {


        if (currentBullets > 0 && autoTimer >= (1f / fireRate))
        {

            selectedWeapon_MuzzleFlash.Play();
            currentBullets--;

            Guns currentGun_Info = selectedGun.GetComponent<ItemData>().Gun;
            int dmg = currentGun_Info.damage;

            if (Physics.Raycast(mainCamera.transform.position,
            mainCamera.transform.forward,
            out raycastHit,
            currentGun_Info.maxRange))
            {
                //Debug.Log("Auto has Hit : " + raycastHit.transform.name);

                if (raycastHit.transform.gameObject != bulletHolePrefab && 
                    raycastHit.transform.name != "Player" && 
                    raycastHit.transform.tag != "Crafting Table")
                {

                    if (raycastHit.transform.tag == "Zombie")
                    {
                        raycastHit.transform.gameObject.GetComponent<ZombieBehavior>().ZombieHit(dmg);
                    }
                    else
                    {

                        //Debug.Log("Auto Hit Tag" + raycastHit.transform.tag);

                        Instantiate(bulletHolePrefab,
                            raycastHit.point + raycastHit.normal * 0.001f,
                            Quaternion.LookRotation(raycastHit.normal, mainCamera.transform.up));

                    }

                }

                
            }

            autoTimer = 0f;

            //Debug.Log(autoTimer);

        }

        autoTimer += Time.deltaTime;

    }

    //Este método é usado para colocar a arma atualmente selecionada visivel.
    //Finaliza o processo de mudança de armas.
    //Torna visível o número de balas do tipo da arma atualmente selecionada. Desativa a wheel das armas no final.
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

    //Este método permite às armas dar reload.
    public void ReloadGun(InputAction.CallbackContext context)
    {

        if(currentMags > 0)
        {

            currentBullets = magCapacity;
            currentMags--;

        }

    }

    //Este método permite equipar uma arma que esteja dropada no mapa.
    //Após equipar a arma, o jogador poderá selecioná-la para a poder usar através da wheel das armas.
    public void EquipGun(GameObject equippedGun)
    {

        if (equippedGun != null)
        {
            //Muda o gameObject para child de gunPosition. De seguida dá reset à posição e à rotação.
            equippedGun.transform.SetParent(gunPosition.transform);

            equippedGun.transform.localPosition = Vector3.zero;
            equippedGun.transform.localRotation = Quaternion.identity;

            //Torna a arma kinematic e põe a colisão em trigger, não permitindo à arma alterar a sua posição/rotação locais
            //nem colidir com outros objetos no mapa.
            equippedGun.GetComponent<Rigidbody>().isKinematic = true;
            equippedGun.GetComponentInChildren<Collider>().isTrigger = true;

            //Desativa a arma, não permitindo ao jogador usá-la até que ele a selecione.
            equippedGun.SetActive(false);

            //Testa o tipo de arma que foi equipada, equipando a arma na wheel das armas e tornando o butão interacionável.
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

    //Este método ativa a wheel das armas.
    public void ChangeGun(InputAction.CallbackContext context)
    {

        changeGunPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

    }

    //Este método é ativado no momento em que o jogador carrega em um butão da wheel das armas e seleciona a arma equipada que está
    //interligada com o butão carregado
    public void SelectGun(Button button)
    {
        
        if (equippedGuns[button.gameObject] == selectedGun) return;

        //Testa se já tem alguma arma selecionada.
        if(selectedGun != null)
        {
            //Se já tiver alguma arma selecionada, desativa-a.
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

            SaveGunInfo(selectedGun.GetComponent<ItemData>().Gun.gunType);

        }

        //Debug.Log(selectedGun);

        //Seleciona a arma dependendo do butão que o jogador carrega
        selectedGun = equippedGuns[button.gameObject];

        //Guarda a info da arma selecionada.
        if(selectedGun != null)
        {
            Guns tempGun = selectedGun.GetComponent<ItemData>().Gun;

            fireRate = tempGun.rateOfFire / 60f;
            magCapacity = tempGun.magCapacity;

            LoadGunInfo(tempGun.gunType);

            selectedWeapon_MuzzleFlash = selectedGun.GetComponentInChildren<ParticleSystem>();

        }

        //Debug.Log(selectedGun);

    }
   
    //Save o número de balas 
    void SaveGunInfo(GunType tempGunType)
    {

        //if(tempGunType == GunType.HandGun)
        //{

            bulletsNumber[tempGunType] = (int)(currentMags * magCapacity + currentBullets) ;

        //}
        //if (tempGunType == GunType.AssaultRifle)
        //{

            bulletsNumber[tempGunType] = (int)(currentMags * magCapacity + currentBullets);

        //}
    }

    //Load o número de balas
    void LoadGunInfo(GunType tempGunType)
    {
        
        //if(tempGunType == GunType.HandGun)
        //{

            currentMags = bulletsNumber[tempGunType] / magCapacity;
            currentBullets = bulletsNumber[tempGunType] % magCapacity;

            if(currentBullets == 0)
            {
                currentBullets = magCapacity;
                currentMags -= 1;
            }

        //}
        //if (tempGunType == GunType.AssaultRifle)
        //{

            currentMags = bulletsNumber[tempGunType] / magCapacity;
            currentBullets = bulletsNumber[tempGunType] % magCapacity;

            if (currentBullets == 0)
            {
                currentBullets = magCapacity;
                currentMags -= 1;
            }

        //}
    }

}


