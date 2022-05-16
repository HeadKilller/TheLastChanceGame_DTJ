using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageButtons : MonoBehaviour
{
    [SerializeField] GameObject InsideCollisionZone;
    [SerializeField] GameObject OutsideCollisionZone;

    [SerializeField] Transform ButtonInsideZone;
    [SerializeField] Transform ButtonOutsideZone;

    [SerializeField] Transform GateTransform;
    OpenCloseGate Gate;
    public bool buttonPressed;

    void Start()
    {
        Gate = GateTransform.GetComponent<OpenCloseGate>();
        InsideCollisionZone.SetActive(false);
        OutsideCollisionZone.SetActive(false);

    }

    void Update()
    {
       if(ButtonOutsideZone.GetComponent<ButtonOpenGate>().activated)
        {
            InsideCollisionZone.SetActive(true);
            Gate.canOpen = true;  
            ButtonOutsideZone.GetComponent<ButtonOpenGate>().activated = false;
        }
        if (ButtonInsideZone.GetComponent<ButtonOpenGate>().activated)
        {
            OutsideCollisionZone.SetActive(true);
            Gate.canOpen = true;
            ButtonInsideZone.GetComponent<ButtonOpenGate>().activated = false;
        }
    }
}
