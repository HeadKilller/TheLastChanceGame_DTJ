using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlacement : MonoBehaviour
{
    public Transform locationOfTrapToBePlacedRelativeToCamera;
    public GameObject trapToBePlaced;
    new Vector3 trapPositionAtSpawnPoint;
    new Quaternion trapRotationAtSpawnPoint;

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            trapPositionAtSpawnPoint = locationOfTrapToBePlacedRelativeToCamera.transform.position;
            trapRotationAtSpawnPoint.eulerAngles = new Vector3(25.0f, 0.0f, 90.0f);
            Instantiate(trapToBePlaced, trapPositionAtSpawnPoint , trapRotationAtSpawnPoint);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            
        }
    }





}
