using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseGate : MonoBehaviour
{
     [SerializeField] GameObject LeftDoor;
     [SerializeField] GameObject RightDoor;
     [SerializeField] float Speed = 5f * 0.2f;

    public bool canOpen;
    public bool canClose;
    public bool isOpenGate;

    void Update()
    {
        if (canOpen && !canClose)
        {
            OpenDoor();
        }
        if (canClose && !canOpen)
        {
            CanClose();
        }

    }
    public void OpenDoor()
    {
        
        LeftDoor.transform.localPosition -= new Vector3(Speed*Time.deltaTime, 0f, 0f);
        RightDoor.transform.localPosition += new Vector3(Speed * Time.deltaTime, 0f, 0f);

        if (LeftDoor.transform.localPosition.x <= 0.4123785f && RightDoor.transform.localPosition.x >= 10.87112f)
        {
            canOpen = false;
            isOpenGate = true;
        }
    }
    public void CanClose()
    {
        LeftDoor.transform.localPosition += new Vector3(Speed * Time.deltaTime, 0f, 0f);
        RightDoor.transform.localPosition -= new Vector3(Speed * Time.deltaTime, 0f, 0f);

        if (LeftDoor.transform.localPosition.x >= 5.641744f && RightDoor.transform.localPosition.x <= 5.641744f)
        {
            canClose = false;
            isOpenGate = false;
            
        }
    }
    
}
