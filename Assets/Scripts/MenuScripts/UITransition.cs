using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class UITransition : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera currentCamera;

    private void Start()
    {
        currentCamera.Priority++;
    }
    public void UpdateCamera(CinemachineVirtualCamera target)
    {
        currentCamera.Priority--;
        currentCamera = target;
        Debug.Log(currentCamera.Name);
        currentCamera.Priority++;
    }
}
