using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class TooltipSystem : MonoBehaviour
{

    [SerializeField] GameObject Tooltip_Canvas;
    [SerializeField] GameObject TooltipInventory_Panel;
    [SerializeField] GameObject TooltipCrafting_Panel;

    [SerializeField] TextMeshProUGUI headerField_Inventory;
    [SerializeField] TextMeshProUGUI contentField_Inventory;

    [SerializeField] LayoutElement layoutElement_Inventory;
    [SerializeField] LayoutElement layoutElement_Crafting;

    [SerializeField] int characterWrapLimit;


    [SerializeField] TextMeshProUGUI headerField_Crafting;
    [SerializeField] TextMeshProUGUI materialNamesContent_Crafting;


    public static TooltipSystem instance;


    RectTransform inventoryTooltip_RectTransform, craftingTooltip_RectTransform;


    private void Start()
    {

        instance = this;
    }

    private void Awake()
    {
        inventoryTooltip_RectTransform = TooltipInventory_Panel.transform as RectTransform;
        craftingTooltip_RectTransform = TooltipCrafting_Panel.transform as RectTransform;
    }

    private void Update()
    {
        Vector2 position = Input.mousePosition;

        //Vector2 offset = new Vector2(-15f, -15f);

        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;


        if(pivotX < 0.5f)
        {

            pivotX = 0f;

            if(pivotY < 0.5f)
            {

                pivotY = 0f;

            }
            else
            {

                pivotY = 1f;

            }

        }
        else
        {

            pivotX = 1f;

            if (pivotY < 0.5f)
            {

                pivotY = 0f;

            }
            else
            {

                pivotY = 1f;

            }


        }

        //Setting the pivots
        inventoryTooltip_RectTransform.pivot = new Vector2(pivotX, pivotY);
        craftingTooltip_RectTransform.pivot = new Vector2(pivotX, pivotY);

        //Setting the positions
        TooltipInventory_Panel.transform.position = position;
        TooltipCrafting_Panel.transform.position = position;

              


    }

    public void ToolTipInventory_Show(string header, string content)
    {

        if (string.IsNullOrEmpty(header))
        {
            headerField_Inventory.gameObject.SetActive(false);
        }
        else
        {
            headerField_Inventory.gameObject.SetActive(true);
            headerField_Inventory.text = header;
        }

        contentField_Inventory.text = content;

        int headerLenght = headerField_Inventory.text.Length;
        int contentLenght = contentField_Inventory.text.Length;

        layoutElement_Inventory.enabled = (headerLenght > characterWrapLimit || contentLenght > characterWrapLimit) ? true : false;


        Tooltip_Canvas.SetActive(true);
        TooltipInventory_Panel.SetActive(true);
    }

    public void ToolTipCrafting_Show(string header, string materialsContent)
    {

        if (string.IsNullOrEmpty(header))
        {
            headerField_Crafting.gameObject.SetActive(false);
        }
        else
        {
            headerField_Crafting.gameObject.SetActive(true);
            headerField_Crafting.text = header;
        }

        materialNamesContent_Crafting.text = materialsContent;

        int headerLenght = headerField_Crafting.text.Length;
        int materialNamesLenght = materialNamesContent_Crafting.text.Length;

        layoutElement_Crafting.enabled = (headerLenght > characterWrapLimit || materialNamesLenght > characterWrapLimit) ? true : false;


        Tooltip_Canvas.SetActive(true);
        TooltipCrafting_Panel.SetActive(true);


    }

    public void ToolTip_Hide()
    {
        Tooltip_Canvas.SetActive(false);
        TooltipCrafting_Panel.SetActive(false);
        TooltipInventory_Panel.SetActive(false);

    }


}
