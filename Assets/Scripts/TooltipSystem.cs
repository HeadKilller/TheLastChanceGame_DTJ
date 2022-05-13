using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TooltipSystem : MonoBehaviour
{

    [SerializeField] GameObject Tooltip_Canvas;

    [SerializeField] TextMeshProUGUI headerField;
    [SerializeField] TextMeshProUGUI contentField;

    [SerializeField] LayoutElement layoutElement;

    [SerializeField] int characterWrapLimit;


    public static TooltipSystem instance;


    private void Start()
    {
        instance = this;
    }

    

    public void ToolTip_Show(string header, string content)
    {

        if (string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }

        contentField.text = content;

        int headerLenght = headerField.text.Length;
        int contentLenght = contentField.text.Length;

        layoutElement.enabled = (headerLenght > characterWrapLimit || contentLenght > characterWrapLimit) ? true : false;


        Tooltip_Canvas.SetActive(true);
    }

    public void ToolTip_Hide()
    {
        Tooltip_Canvas.SetActive(false);
    }


}
