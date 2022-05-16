using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageCloseAuto : MonoBehaviour
{
    [SerializeField] GameObject CloseInInside;
    [SerializeField] GameObject CloseInOutside;
    //TODO: Programar a fechar caso o gate foi aberto de um determinado lado.
    CloseAutoCheck canCloseInside;
    CloseAutoCheck canCloseOutside;
    OpenCloseGate Gate;
    void Start()
    {
        canCloseInside = CloseInInside.GetComponent<CloseAutoCheck>();
        canCloseOutside = CloseInOutside.GetComponent<CloseAutoCheck>();
        Gate = this.GetComponent<OpenCloseGate>();

    }


    void Update()
    {

        if (!Gate.isOpenGate)
        {
            Gate.canClose = false;
        }
        if (canCloseInside.canClose == true && Gate.isOpenGate)
       {
            Gate.canClose = true;
            canCloseInside.canClose = false;
            
       }
      if (canCloseOutside.canClose == true && Gate.isOpenGate)
      {
            Gate.canClose = true;
            canCloseOutside.canClose = false;
      }
    }
    
    
}
