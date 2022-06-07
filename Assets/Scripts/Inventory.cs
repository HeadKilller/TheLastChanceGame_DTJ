using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;


public class Inventory : MonoBehaviour
{

    [SerializeField] GameObject inventoryGameObject;
    [SerializeField] GameObject inventorySlotsParent;

    [SerializeField] GameObject gunsWheel_Panel;

    [SerializeField] GameObject confirmationWindow_GameObject;
 
    [SerializeField] Slider confirmationWindow_Slider;

    
    [SerializeField] int inventorySize;

    public static Inventory instance;

    PlayerInputControl playerInputControl;
       

    int slotMaxCapacity;

    GameObject gameObjectToDestroy;

    public bool MaskEquiped;

    List<GameObject> inventory_Slots;
    List<Items> inventorySlotsContent;
    public List<GameObject> inventorySlotsContent_Objects;
    List<int> inventorySlotsCurrentCapacity;

    public bool IsMovingItem;
    GameObject SlotWhereToMove;
    GameObject SlotFromWhereToMove;
    public List<GameObject> Inventory_Slots
    {
        get { return inventory_Slots; }
    }
    public List<Items> Inventory_SlotsContent
    {
        get { return inventorySlotsContent; }
    }
    public List<GameObject> Inventory_SLotsContent_Objects
    {
        get { return inventorySlotsContent_Objects; }

    }
    public GameObject InvPanel
    {
        get { return inventorySlotsParent; }
    }
    public GameObject GunsWheelPanel
    {
        get { return gunsWheel_Panel; }
    }

    private void Awake()
    {
        #region Variables Initialization

        instance = this;

        inventory_Slots = new List<GameObject>();
        inventorySlotsContent = new List<Items>();
        inventorySlotsContent_Objects = new List<GameObject>();
        inventorySlotsCurrentCapacity = new List<int>();

        
        

        #endregion

        #region Variables First Values

        gameObjectToDestroy = null;
        SlotWhereToMove = null;
        SlotFromWhereToMove = null;
        slotMaxCapacity = 99;

        int count = 1;

        IsMovingItem = false;

        foreach (Transform child in inventorySlotsParent.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.name == "InventorySlot" + count.ToString())
            {

                inventory_Slots.Add(child.gameObject);
                inventorySlotsContent.Add(null);
                inventorySlotsContent_Objects.Add(null);
                inventorySlotsCurrentCapacity.Add(0);

                
                count++;
            }
        }

        #endregion

        #region Player Input

        playerInputControl = new PlayerInputControl();

        playerInputControl.UI.Enable();

        playerInputControl.UI.Inventory.performed += InventoryOpen_Close;

        #endregion

        #region Button Listeners

        confirmationWindow_Slider.onValueChanged.AddListener(delegate { ChangeSliderValue(); });

        #endregion
    }


    private void Update()
    {

        if (inventoryGameObject.activeSelf)
        {

            CheckIfIsOverUI();

        }

    }




    #region Inventory

    //Função chamada para adicionar itens ao inventário, sem uma slot específica para adicionar.
    public void AddItem(GameObject _gameObject, Items item, GameObject itemObject, bool toDestroy)
    {

        bool foundEmptySlot = false;

        if (item.isStackable)
        {

            for(int i = 0; i < inventory_Slots.Count; i++)
            {

                if (inventorySlotsContent[i] == null) continue;

                if (inventorySlotsContent[i] == item && inventorySlotsCurrentCapacity[i] < slotMaxCapacity)
                {

                    inventorySlotsCurrentCapacity[i]++;
                    TextMeshProUGUI text = inventory_Slots[i].GetComponentInChildren<TextMeshProUGUI>();
                    if (text != null)
                        text.SetText(inventorySlotsCurrentCapacity[i].ToString());

                    foundEmptySlot = true;
                }

            }
                                    

        }

        if (!foundEmptySlot)
        {

            for(int i = 0; i < inventory_Slots.Count; i++)
            {

                if(inventorySlotsContent[i] == null)
                {


                    inventorySlotsContent[i] = item;
                    inventorySlotsContent_Objects[i] = itemObject;
                    
                    //Debug.Log("Item : " + inventorySlotsContent_Objects[i]);
                    //Debug.Log("Num : " + i);

                    inventorySlotsCurrentCapacity[i]++;

                    TextMeshProUGUI text = inventory_Slots[i].GetComponentInChildren<TextMeshProUGUI>();
                    if (text != null)
                        text.SetText(inventorySlotsCurrentCapacity[i].ToString());

                    foundEmptySlot=true;
                    //Debug.Log(inventory_Slots[i].name);
                    ActivateSlot(inventory_Slots[i], i);
                    break;
                }

            }
                        

        }

        if (!foundEmptySlot) return;

        if(toDestroy)
            Destroy(_gameObject);
        else
            _gameObject.SetActive(false);
        //Debug.Log("Picking up " + item.name);

    }

    //Função chamada para adicionar itens a uma slot específica do inventário.
    public void AddItemToSlot(GameObject slotToAdd, Items item, GameObject itemObject, int quantityToAdd)
    {

        int index = inventory_Slots.IndexOf(slotToAdd);

        if(inventorySlotsContent[index] == null)
        {

            inventorySlotsContent[index] = item;
            inventorySlotsContent_Objects[index] = itemObject;
            inventorySlotsCurrentCapacity[index] += quantityToAdd;
            ActivateSlot(slotToAdd, index);

        }
        else if (item.isStackable && inventorySlotsContent[index] == item)
        {
            inventorySlotsContent[index] = item;
            inventorySlotsCurrentCapacity[index] += quantityToAdd;
            //ActivateSlot(slotToAdd, index);
        }

        TextMeshProUGUI text = inventory_Slots[index].GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
            text.SetText(inventorySlotsCurrentCapacity[index].ToString());


    }
   
    //Função chamada quando se move itens no inventário.
    public void RemoveItem(GameObject slot, bool isMovingItem, GameObject slotToMove)
    {


        #region TO RESOLVE

        /*if (inventorySlotCurrentCapacity[_gameObject] > 1)
        {
            int numberToDestroy = N_ofItemstoDestroyWindow(_gameObject);


            inventorySlotCurrentCapacity[_gameObject] -= numberToDestroy;
        }
        else
        {
            inventory_with_Item[_gameObject] = null;
            inventorySlotCurrentCapacity[_gameObject] = 0;

            DeActivateSlot(_gameObject);

        }
        */
        #endregion

        IsMovingItem = isMovingItem;
        SlotWhereToMove = slotToMove;
        if(isMovingItem)
            SlotFromWhereToMove = slot;

        int index = -1;

        for(int i = 0; i < inventory_Slots.Count; i++)
        {

            if (inventory_Slots[i] == slot)
                index = i;
        }

        confirmationWindow_GameObject.SetActive(true);

        confirmationWindow_Slider.maxValue = inventorySlotsCurrentCapacity[index];

        gameObjectToDestroy = slot;

        
    }

    //Função ao chamar quando se carrega no botão para remover itens do inventário.
    public void RemoveItem(GameObject slot)
    {
        int index = -1;

        for (int i = 0; i < inventory_Slots.Count; i++)
        {

            if (inventory_Slots[i] == slot)
                index = i;
        }

        confirmationWindow_GameObject.SetActive(true);

        confirmationWindow_Slider.maxValue = inventorySlotsCurrentCapacity[index];

        gameObjectToDestroy = slot;

    }

    //Função chamada ao dar craft de itens.
    public void RemoveItem(Items item, int quantityToRemove)
    {

        GameObject itemToRemove = null;
        int index = -1;
        for(int i = 0; i < inventory_Slots.Count; i++)
        {

            if (inventorySlotsContent[i] == null) continue;

            if(inventorySlotsContent[i] == item)
            {
                itemToRemove = inventory_Slots[i];
                index = i;
                break;
            }
            if (itemToRemove == null) return;

        }
                

        TextMeshProUGUI text = itemToRemove.GetComponentInChildren<TextMeshProUGUI>();
        inventorySlotsCurrentCapacity[index] -= quantityToRemove;

        //Debug.Log("Item : " + inventorySlotsContent[index]);
        //Debug.Log("Quantity : " + inventorySlotsCurrentCapacity[index]);

        if (inventorySlotsCurrentCapacity[index] <= 0)
        {
            inventorySlotsCurrentCapacity[index] = 0;
            text.SetText("");
            DeActivateSlot(itemToRemove);
        }
        else
        {
            text.SetText(inventorySlotsCurrentCapacity[index].ToString());
        }

    }

    //Função chamada ao carregar na tecla para abrir/fechar o inventário.
    public void InventoryOpen_Close(InputAction.CallbackContext context)
    {
        inventoryGameObject.SetActive(!inventoryGameObject.activeSelf);
        gunsWheel_Panel.SetActive(!gunsWheel_Panel.activeSelf);

        if (inventoryGameObject.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

        }


    }

    //Função chamada ao carregar no botão para fechar o inventário.
    public void Close_Inventory()
    {
        inventoryGameObject.SetActive(false);
        gunsWheel_Panel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    //Função chamada para ativar as slots do inventário.
    private void ActivateSlot(GameObject _gameObject, int slot)
    {
        

        foreach (Transform child in _gameObject.GetComponentsInChildren<Transform>())
        {
            //Debug.Log(child.name);
            if (child.name == "SlotButton")
            {                  
                child.GetComponent<Button>().enabled = true;
            }

            if(child.name.Contains("SlotImage"))
            {
                child.GetComponent<Image>().sprite = inventorySlotsContent[slot].icon;
                child.GetComponent<Image>().enabled = true;
                child.GetComponent<ItemDragHandler>().enabled = true;
            }

            if (child.name == "EmptySlotButton")
            {
                child.GetComponent<Image>().enabled = true;
                child.GetComponent<Button>().enabled = true;
            }

        }
    }

    //Função chamada para desativar as slots do inventário.
    private void DeActivateSlot(GameObject _gameObject)
    {
        int index = -1;

        for(int i = 0; i < inventory_Slots.Count; i++)
        {

            if (inventory_Slots[i].gameObject == _gameObject)
                index = i;

        }

        //Debug.Log("Destroying... : " + inventory_Slots[index]);
        inventorySlotsContent[index] = null;
        inventorySlotsContent_Objects[index] = null;

        foreach (Transform child in _gameObject.GetComponentsInChildren<Transform>())
        {
            if (child.name == "SlotButton")
            {                
                child.GetComponent<Button>().enabled = false;
            }
            if (child.name.Contains("SlotImage"))
            {
                child.GetComponent<Image>().sprite = null;
                child.GetComponent<Image>().enabled = false;
                child.GetComponent<ItemDragHandler>().enabled = false;
            }
            if (child.name == "EmptySlotButton")
            {
                child.GetComponent<Image>().enabled = false;
                child.GetComponent<Button>().enabled = false;
            }
        }
    }

    //Função chamada no momento em que se confirma quantos itens para remover/mover.
    public void DestroyItem()
    {

        int index = -1;
        for ( int i = 0; i < inventory_Slots.Count; i++)
        {
            if (inventory_Slots[i] == gameObjectToDestroy)
                index = i;
        }

        if (confirmationWindow_Slider.value <= inventorySlotsCurrentCapacity[index])
            inventorySlotsCurrentCapacity[index] -= (int)confirmationWindow_Slider.value;
        else
            inventorySlotsCurrentCapacity[index] = 0;

        //Caso esteja a mover um item de um slot para outro.
        if (IsMovingItem)
        {
            Debug.Log("Add item : " + inventorySlotsContent_Objects[index] + " from slot : " + index);

            AddItemToSlot(SlotWhereToMove, inventorySlotsContent[index], inventorySlotsContent_Objects[index], (int)confirmationWindow_Slider.value);
            if (inventorySlotsCurrentCapacity[index] == 0)
            {
                DeActivateSlot(SlotFromWhereToMove);
                SlotFromWhereToMove = null;
                SlotWhereToMove = null;
                IsMovingItem = false;
            }
        }


        TextMeshProUGUI text = gameObjectToDestroy.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null && inventorySlotsCurrentCapacity[index] != 0)
            text.SetText(inventorySlotsCurrentCapacity[index].ToString());
        if (text != null && inventorySlotsCurrentCapacity[index] == 0)
            text.SetText("");

        if(inventorySlotsCurrentCapacity[index] == 0)
        {
            DeActivateSlot(gameObjectToDestroy);
        }

        confirmationWindow_Slider.value = 0;

        confirmationWindow_GameObject.SetActive(false);

    }
    
    //Função chamada para mudar o número que mostra quantos itens selecionados no slider na janela de confirmação do inventário.
    public void ChangeSliderValue()
    {

        TMP_Text textTMP = null;

        foreach (var child in confirmationWindow_GameObject.GetComponentsInChildren<TMP_Text>()){
            if (child.name == "SliderValue")
                textTMP = child;

        }

        int value = (int)confirmationWindow_Slider.value;

        //Debug.Log(value);

        if (textTMP != null)
            textTMP.text = value.ToString();
    }

    //Função chamada quando se está a fazer Craft de itens. Verifica se tem um específico número de certos materiais no inventário.
    public bool CheckIfHasInInventory(Items material, int materialNum)
    {

        bool canCraft = false;

        for(int i = 0; i < inventory_Slots.Count; i++)
        {

            if(inventorySlotsContent[i] != null && inventorySlotsContent[i] == material)
            {

                string materialQuantity_String = inventory_Slots[i].GetComponentInChildren<TextMeshProUGUI>().text;

                int materialQuantity_Integer = Int32.Parse(materialQuantity_String);

                if (materialQuantity_Integer >= materialNum)
                {
                    canCraft = true;
                }
            }

        }
                

        return canCraft;

    }

    #endregion

    void CheckIfIsOverUI()
    {

        //RaycastHit hit;

        //Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        ////Debug.Log("mouse position : " + mouseRay.origin);


        //if (Physics.Raycast(mouseRay, out hit) && hit.collider.tag == "UI")
        //{
        //    //Debug.Log("Is over : " + hit.collider.gameObject.name);

        //    for(int i = 0; i < inventory_Slots.Count; i++)
        //    {

        //        if(hit.collider.gameObject == inventory_Slots[i] && inventorySlotsContent[i] != null)
        //        {



        //        }

        //    }

            

        //}

    }

    //Função chamada para verificar que item o rato está em cima. Serve para mostrar os tooltips.
    public Items CheckItemOnHover(GameObject inventorySlot)
    {

        Items itemOnHover = null;

        for(int i = 0; i < inventorySize; i++)
        {

            if(inventory_Slots[i] == inventorySlot)
            {

                itemOnHover = inventorySlotsContent[i];

            }

        }

        return itemOnHover;

    }

    

}
