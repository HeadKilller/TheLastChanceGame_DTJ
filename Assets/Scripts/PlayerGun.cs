using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerGun : MonoBehaviour
{

    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject gunPosition;
    [SerializeField] GameObject bulletHolePrefab;

    [SerializeField] GameObject recoilGameObject;

    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] ParticleSystem hitParticleSystem;

    [SerializeField] GameObject currentBulletsPanel;
    [SerializeField] GameObject changeGunPanel;

    [SerializeField] GameObject handGun_Slot;
    [SerializeField] GameObject assaultGun_Slot;
    [SerializeField] GameObject unarmed_Slot;
    [SerializeField] GameObject smg_Slot;
   
    [Header("Sounds")]
    [SerializeField] AudioClip audioCLip_AK;
    [SerializeField] AudioClip audioClip_HandGun;
    [SerializeField] AudioClip audioClip_UMP;

    AudioSource audioSource;

    Dictionary<GameObject, GameObject> equippedGuns;
    Dictionary<GunType, int> bulletsNumber;

    ParticleSystem selectedWeapon_MuzzleFlash;


    PlayerInputControl playerInputControl;
    GunsRecoil Recoil;

    GameObject selectedGun;

    TMP_Text currentBulletsText;
    Image bulletsIcon;

    RaycastHit raycastHit;

    Vector3 gunRecoil;

    float autoTimer;
    float fireRate;
    float magCapacity;

    float currentBullets, currentMags;


    public static PlayerGun instance;

    private bool shooted;
    bool isChangingGun;
    public bool isPauseMenuActivated;

    #region Propriedades

    public GameObject SelectedGun
    {
        get { return selectedGun; }
    }

    #endregion


    private void Start()
    {
        bulletsNumber = new Dictionary<GunType, int>();
        equippedGuns = new Dictionary<GameObject, GameObject>();

        equippedGuns.Add(handGun_Slot, null);
        equippedGuns.Add(assaultGun_Slot, null);
        equippedGuns.Add(smg_Slot, null);

        unarmed_Slot.GetComponent<Button>().interactable = true;
        isChangingGun = false;
        isPauseMenuActivated = false;

        Recoil = recoilGameObject.GetComponent<GunsRecoil>();

        gunRecoil = Vector3.zero;

        currentBulletsText = currentBulletsPanel.GetComponentInChildren<TMP_Text>();
        bulletsIcon = currentBulletsPanel.GetComponentInChildren<Image>();

        audioSource = this.GetComponent<AudioSource>();

        bulletsNumber.Add(GunType.HandGun, 14);
        bulletsNumber.Add(GunType.AssaultRifle, 60);
        bulletsNumber.Add(GunType.SMG, 60);

        currentBulletsText.color = new Color(1, 1, 1);

        selectedWeapon_MuzzleFlash = null;
        selectedGun = null;

        autoTimer = 0;
        fireRate = 0;
        magCapacity = 0;
        currentBullets = 0;
        currentMags = 0;


        instance = this;

    }

    private void Awake()
    {

        playerInputControl = new PlayerInputControl();       

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

        if (selectedGun != null && currentBullets > 0 && !changeGunPanel.activeInHierarchy && !Inventory.instance.Inventory_Canvas.activeSelf && !Craft.instance.CraftingMenuCanvas.activeSelf && !isChangingGun && !isPauseMenuActivated)
        {

            selectedWeapon_MuzzleFlash.Play();
            currentBullets--;

            Recoil.RecoilFire(gunRecoil);

            PlaySound();

            Guns currentGun_Info = selectedGun.GetComponent<ItemData>().Gun;

            int dmg = currentGun_Info.damage;

            if (Physics.Raycast(mainCamera.transform.position,
                mainCamera.transform.forward,
                out raycastHit,
                currentGun_Info.maxRange))
            {
                //Debug.Log("Semi has Hit : " + raycastHit.transform.name);

                if (raycastHit.transform.gameObject != bulletHolePrefab &&
                    raycastHit.transform.tag != "Player" &&
                    raycastHit.transform.tag != "Crafting Table" &&
                    raycastHit.transform.tag != "Zombie")
                {

                    GameObject tempDecal = Instantiate(bulletHolePrefab,
                        raycastHit.point + raycastHit.normal * 0.001f,
                        Quaternion.LookRotation(raycastHit.normal, mainCamera.transform.up));
                    Destroy(tempDecal, 10f);
                }

                if (raycastHit.transform.tag == "Zombie")
                {
                    if (raycastHit.transform.name == "Head")
                    {
                        Debug.Log("Critic. Head");
                        raycastHit.transform.gameObject.GetComponentInParent<ZombieBehavior>().ZombieHit(dmg * 5);
                    }
                    else
                    {
                        raycastHit.transform.gameObject.GetComponent<ZombieBehavior>().ZombieHit(dmg);

                    }


                }
            }

        }


    }
    private void PlaySound()
    {

        switch (selectedGun.GetComponent<ItemData>().Gun.gunType)
        {

            case GunType.HandGun:

                audioSource.PlayOneShot(audioClip_HandGun);

                break;
            case GunType.AssaultRifle:

                audioSource.PlayOneShot(audioCLip_AK);

                break;
            case GunType.SMG:

                audioSource.PlayOneShot(audioClip_UMP);

                break;
        }
            



    }

    //Método chamado quando o jogador mantém o botão do rato pressionado
    public void FireAuto()
    {


        if (currentBullets > 0 && autoTimer >= (1f / fireRate) && !changeGunPanel.activeInHierarchy && !Inventory.instance.Inventory_Canvas.activeSelf && !Craft.instance.CraftingMenuCanvas.activeSelf && !isChangingGun && !isPauseMenuActivated)
        {
            selectedWeapon_MuzzleFlash.Play();
            currentBullets--;
            Recoil.RecoilFire(gunRecoil);   

            PlaySound();

            //Debug.Log(/*mainCamera.transform.position + */mainCamera.transform.forward);

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
                    raycastHit.transform.tag != "Crafting Table" && 
                    raycastHit.transform.tag != "Player")
                {

                    if (raycastHit.transform.tag == "Zombie")
                    {
                        if(raycastHit.transform.name == "Head")
                        {
                            Debug.Log("Critic. Head");
                            raycastHit.transform.gameObject.GetComponentInParent<ZombieBehavior>().ZombieHit(dmg * 5);
                        }
                        else
                        {
                            raycastHit.transform.gameObject.GetComponent<ZombieBehavior>().ZombieHit(dmg);
                        }
                    }
                    else
                    {
                        //Debug.Log("Auto Hit Tag" + raycastHit.transform.tag);

                        GameObject tempDecal = Instantiate(bulletHolePrefab,
                            raycastHit.point + raycastHit.normal * 0.001f,
                            Quaternion.LookRotation(raycastHit.normal, mainCamera.transform.up));

                        Destroy(tempDecal, 10f);

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
        Camera.main.GetComponent<PlayerLook>().enabled = true;
        isChangingGun = false;

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
                case GunType.SMG:

                    smg_Slot.GetComponent<Button>().interactable = false;
                    break;


            }
            
            currentBulletsText.text = currentBullets.ToString() + " / " + currentMags.ToString();

            currentBulletsPanel.SetActive(true);


        }

        

        changeGunPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Debug.Log("Gun Selected");

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

    ////Este método permite equipar uma arma que esteja dropada no mapa.
    ////Após equipar a arma, o jogador poderá selecioná-la para a poder usar através da wheel das armas.
    //public void EquipGun(GameObject equippedGun)
    //{

    //    if (equippedGun != null)
    //    {
    //        //Muda o gameObject para child de gunPosition. De seguida dá reset à posição e à rotação.
    //        equippedGun.transform.SetParent(gunPosition.transform);

    //        equippedGun.transform.localPosition = Vector3.zero;
    //        equippedGun.transform.localRotation = Quaternion.identity;

    //        //Torna a arma kinematic e põe a colisão em trigger, não permitindo à arma alterar a sua posição/rotação locais
    //        //nem colidir com outros objetos no mapa.
    //        equippedGun.GetComponent<Rigidbody>().isKinematic = true;
    //        equippedGun.GetComponentInChildren<Collider>().isTrigger = true;

    //        //Desativa a arma, não permitindo ao jogador usá-la até que ele a selecione.
    //        equippedGun.SetActive(false);

    //        //Testa o tipo de arma que foi equipada, equipando a arma na wheel das armas e tornando o butão interacionável.
    //        switch (equippedGun.GetComponent<ItemData>().Gun.gunType)
    //        {
    //            case GunType.HandGun:

    //                equippedGuns[handGun_Slot] = equippedGun;
    //                handGun_Slot.GetComponent<Button>().interactable = true;
    //                break;
    //            case GunType.AssaultRifle:

    //                equippedGuns[assaultGun_Slot] = equippedGun;
    //                assaultGun_Slot.GetComponent<Button>().interactable = true;
    //                break;
    //        }

    //    }

    //}

    public void EquipGun(GameObject toEquipGun)
    {

        if(toEquipGun != null)
        {

            //Muda o gameObject para child de gunPosition. De seguida dá reset à posição e à rotação.
            toEquipGun.transform.SetParent(gunPosition.transform);

            

            toEquipGun.transform.localPosition = Vector3.zero;
            toEquipGun.transform.localRotation = Quaternion.identity;

            //Torna a arma kinematic e põe a colisão em trigger, não permitindo à arma alterar a sua posição/rotação locais
            //nem colidir com outros objetos no mapa.
            toEquipGun.GetComponent<Rigidbody>().isKinematic = true;
            toEquipGun.GetComponentInChildren<Collider>().isTrigger = true;

            //Desativa a arma, não permitindo ao jogador usá-la até que ele a selecione.
            toEquipGun.SetActive(false);

            Debug.Log(toEquipGun);

            toEquipGun.GetComponent<Target>().enabled = false;
            toEquipGun.GetComponent<Outline>().enabled = false;

            switch (toEquipGun.GetComponent<ItemData>().Gun.gunType)
            {
                case GunType.HandGun:

                    //Debug.Log("Hand Gun : " + equippedGuns[handGun_Slot]);

                    if(equippedGuns[handGun_Slot] == null)
                    {
                        equippedGuns[handGun_Slot] = toEquipGun;
                        handGun_Slot.GetComponent<Button>().interactable = true;

                        foreach(var objs in handGun_Slot.GetComponentsInChildren<Transform>())
                        {

                            if(objs.name == "Slot Icon")
                            {

                                Image tempImage = objs.GetComponent<Image>();
                                tempImage.enabled = true;
                                tempImage.sprite = toEquipGun.GetComponent<ItemData>().Gun.icon;

                            }

                        }

                    }
                    else
                    {
                        GameObject currentEquippedGun = equippedGuns[handGun_Slot];

                        currentEquippedGun.GetComponent<Target>().enabled = true;
                        currentEquippedGun.GetComponent<Outline>().enabled = false;

                        Inventory.instance.AddItem(currentEquippedGun, currentEquippedGun.GetComponent<ItemData>().Gun, currentEquippedGun, false);
                        equippedGuns[handGun_Slot] = toEquipGun;
                        handGun_Slot.GetComponent<Button>().interactable = true;

                        foreach (var objs in handGun_Slot.GetComponentsInChildren<Transform>())
                        {

                            if (objs.name == "Slot Icon")
                            {

                                Image tempImage = objs.GetComponent<Image>();
                                tempImage.enabled = true;
                                tempImage.sprite = toEquipGun.GetComponent<ItemData>().Gun.icon;

                            }

                        }

                    }

                    break;
                case GunType.AssaultRifle:

                    if (equippedGuns[assaultGun_Slot] == null)
                    {
                        equippedGuns[assaultGun_Slot] = toEquipGun;
                        assaultGun_Slot.GetComponent<Button>().interactable = true;

                        foreach (var objs in assaultGun_Slot.GetComponentsInChildren<Transform>())
                        {

                            if (objs.name == "Slot Icon")
                            {

                                Image tempImage = objs.GetComponent<Image>();
                                tempImage.enabled = true;
                                tempImage.sprite = toEquipGun.GetComponent<ItemData>().Gun.icon;

                            }

                        }
                    }
                    else
                    {
                        GameObject currentEquippedGun = equippedGuns[assaultGun_Slot];

                        currentEquippedGun.GetComponent<Target>().enabled = true;
                        currentEquippedGun.GetComponent<Outline>().enabled = false;

                        Inventory.instance.AddItem(currentEquippedGun, currentEquippedGun.GetComponent<ItemData>().Gun, currentEquippedGun, false);
                        equippedGuns[assaultGun_Slot] = toEquipGun;
                        assaultGun_Slot.GetComponent<Button>().interactable = true;

                        foreach (var objs in assaultGun_Slot.GetComponentsInChildren<Transform>())
                        {

                            if (objs.name == "Slot Icon")
                            {

                                Image tempImage = objs.GetComponent<Image>();
                                tempImage.enabled = true;
                                tempImage.sprite = toEquipGun.GetComponent<ItemData>().Gun.icon;

                            }

                        }

                    }
                   
                    break;

                case GunType.SMG:

                    if(equippedGuns[smg_Slot] == null)
                    {

                        equippedGuns[smg_Slot] = toEquipGun;
                        smg_Slot.GetComponent<Button>().interactable = true;

                        foreach (var objs in smg_Slot.GetComponentsInChildren<Transform>())
                        {

                            if (objs.name == "Slot Icon")
                            {

                                Image tempImage = objs.GetComponent<Image>();
                                tempImage.enabled = true;
                                tempImage.sprite = toEquipGun.GetComponent<ItemData>().Gun.icon;

                            }

                        }

                    }
                    else
                    {
                        GameObject currentEquippedGun = equippedGuns[smg_Slot];

                        currentEquippedGun.GetComponent<Target>().enabled = true;
                        currentEquippedGun.GetComponent<Outline>().enabled = false;

                        Inventory.instance.AddItem(currentEquippedGun, currentEquippedGun.GetComponent<ItemData>().Gun, currentEquippedGun, false);
                        equippedGuns[smg_Slot] = toEquipGun;
                        smg_Slot.GetComponent<Button>().interactable = true;

                        foreach (var objs in smg_Slot.GetComponentsInChildren<Transform>())
                        {

                            if (objs.name == "Slot Icon")
                            {

                                Image tempImage = objs.GetComponent<Image>();
                                tempImage.enabled = true;
                                tempImage.sprite = toEquipGun.GetComponent<ItemData>().Gun.icon;

                            }

                        }
                    }

                    break;
            }

        }

    }

    //Este método ativa a wheel das armas.
    public void ChangeGun(InputAction.CallbackContext context)
    {
        Camera.main.GetComponent<PlayerLook>().enabled = false;
        isChangingGun = true;
        changeGunPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

    }

    //Este método é ativado no momento em que o jogador carrega em um butão da wheel das armas e seleciona a arma equipada que está
    //interligada com o butão carregado
    public void SelectGun(Button button)
    {

        //Debug.Log("Select Gun");

        if(button.gameObject == unarmed_Slot)
        {

            if(selectedGun != null)
            {
                GunType tempGunType = selectedGun.GetComponent<ItemData>().Gun.gunType;

                switch (tempGunType)
                {
                    case GunType.HandGun:
                        handGun_Slot.GetComponent<Button>().interactable = true;
                        break;
                    case GunType.AssaultRifle:
                        assaultGun_Slot.GetComponent<Button>().interactable = true;
                        break;
                    case GunType.SMG:
                        smg_Slot.GetComponent<Button>().interactable = true;
                        break;
                }

                selectedGun.gameObject.SetActive(false);
                SaveGunInfo(tempGunType);
                selectedGun = null;

                currentBulletsPanel.SetActive(false);
            }

            return;
        }

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

                case GunType.SMG:
                    smg_Slot.GetComponent<Button>().interactable = true;
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

            gunRecoil = tempGun.recoil;

            LoadGunInfo(tempGun.gunType);

            selectedWeapon_MuzzleFlash = selectedGun.GetComponentInChildren<ParticleSystem>();

        }

        //Debug.Log(selectedGun);
        //Debug.Log("Gun Chosen : " + selectedGun);

    }

    public void PickMunition(Items tempGun)
    {

        Debug.Log("Picking Munition. Type : " + tempGun.munitionsType);

        if(selectedGun != null)
        {

            GunType tempGunType = selectedGun.GetComponent<ItemData>().Gun.gunType;
            SaveGunInfo(tempGunType);

            switch (tempGun.munitionsType)
            {
                case GunType.HandGun:

                    bulletsNumber[GunType.HandGun] += 14;

                    break;

                case GunType.AssaultRifle:

                    bulletsNumber[GunType.AssaultRifle] += 60;

                    break;

                case GunType.SMG:

                    bulletsNumber[GunType.SMG] += 60;

                    break;
            }

            LoadGunInfo(tempGunType);

        }
        else
        {
            switch (tempGun.munitionsType)
            {
                case GunType.HandGun:

                    bulletsNumber[GunType.HandGun] += 14;

                    break;

                case GunType.AssaultRifle:

                    bulletsNumber[GunType.AssaultRifle] += 60;

                    break;

                case GunType.SMG:

                    bulletsNumber[GunType.SMG] += 60;

                    break;
            }
        }
    }

    //Save o número de balas 
    void SaveGunInfo(GunType tempGunType)
    {


         bulletsNumber[tempGunType] = (int)(currentMags * magCapacity + currentBullets) ;

        
    }

    //Load o número de balas
    void LoadGunInfo(GunType tempGunType)
    {       

            currentMags = (int) (bulletsNumber[tempGunType] / magCapacity);
            currentBullets = (int) (bulletsNumber[tempGunType] % magCapacity);

            if(currentBullets == 0 && currentMags > 0)
            {
                currentBullets = magCapacity;
                currentMags -= 1;
            }

        
    }

    
}


