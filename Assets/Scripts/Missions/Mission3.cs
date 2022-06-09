using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission3 : MonoBehaviour
{
    [Header("Basics")]
    [SerializeField] GameObject Heli;
    [SerializeField] GameObject Rope;

    [Header("Text Waypoints")]
    [SerializeField] GameObject GlobalObjetive;
    [SerializeField] GameObject FinalObjetive;

    [SerializeField] GameObject EndText;

    public bool callBackUp;


    void Start()
    {
        GlobalObjetive.SetActive(true);
    } 
    void Update()
    {
        
        if (callBackUp)
        {
            StartFinalObjective();
            if(Rope.GetComponent<TouchRope>().canEnd)
            {
                EndText.SetActive(true);
                FinalObjetive.SetActive(false);

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0f;
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
