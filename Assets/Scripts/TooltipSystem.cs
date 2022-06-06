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
    [SerializeField] TextMeshProUGUI materialNumsContent_Crafting;


    public static TooltipSystem instance;


    private void Start()
    {
        instance = this;
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

    public void ToolTipCrafting_Show(string header, string materialNames, string materialNums)
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

        materialNamesContent_Crafting.text = materialNames;
        materialNumsContent_Crafting.text = materialNums;

        int headerLenght = headerField_Crafting.text.Length;
        int materialNamesLenght = materialNamesContent_Crafting.text.Length;
        int materialNumsLenght = materialNumsContent_Crafting.text.Length;

        layoutElement_Crafting.enabled = (headerLenght > characterWrapLimit || materialNamesLenght + materialNumsLenght > characterWrapLimit) ? true : false;

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
