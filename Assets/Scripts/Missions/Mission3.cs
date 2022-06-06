using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission3 : MonoBehaviour
{
    [Header("Basics")]
    [SerializeField] GameObject Radio;
    [SerializeField] GameObject Heli;
    [SerializeField] GameObject Rope;

    [Header("Text Waypoints")]
    [SerializeField] GameObject GlobalObjetive;
    [SerializeField] GameObject FinalObjetive;

    [SerializeField] GameObject EndText;

    private RadioFixed isFixed;


    void Start()
    {
       // GlobalObjetive.SetActive(true);
        isFixed = Radio.GetComponent<RadioFixed>();
        GlobalObjetive.SetActive(true);
    } 
    void Update()
    {
        
        if (isFixed.RadioFix)
        {
            StartFinalObjective();
            if(Rope.GetComponent<TouchRope>().canEnd)
            {
                EndText.SetActive(true);
                FinalObjetive.SetActive(false);
            }
        }
    }
    void StartFinalObjective()
    {
        GlobalObjetive.SetActive(false);
        FinalObjetive.SetActive(true);
        Heli.SetActive(true);

        Heli.GetComponent<SetNewPosition>().canMove = true;
    
    }
}
