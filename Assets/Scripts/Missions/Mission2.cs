using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission2 : MonoBehaviour
{
    [Header("Basics")]
    [SerializeField] GameObject StartMissionText;
    [Header("Waypoints")]
    [SerializeField] Transform StartMission;
    [SerializeField] Transform FristWaypoint;
    [SerializeField] Transform SecondWaypoint;
    [Header("Text Waypoints")]
    [SerializeField] GameObject TextWaypointFrist;
    [SerializeField] GameObject TextWaypointSecond;

    //----------
    WayPointMission FristCheckpoint, SecondCheckpoint;
    GunPicked PickUpGun;
    public bool MissionEnded;

    private void Start()
    {
        StartMissionText.SetActive(true);
        PickUpGun = StartMission.GetComponent<GunPicked>();
        FristCheckpoint = FristWaypoint.GetComponent<WayPointMission>();
        SecondCheckpoint = SecondWaypoint.GetComponent<WayPointMission>();
    }

    void Update()
    {
        MissionStart();
        Checkpoint_One();
        Checkpoint_Second();
    }

    void MissionStart()
    {
        if(PickUpGun.pickGun)
        {
            StartMissionText.SetActive(false);
            StartMission.gameObject.SetActive(false);
            FristWaypoint.gameObject.SetActive(true);
            TextWaypointFrist.SetActive(true); 
        }
    }
    void Checkpoint_One()
    {
        if(FristCheckpoint.updateObjective)
        {
            FristWaypoint.gameObject.SetActive(false);
            TextWaypointFrist.SetActive(false);
            TextWaypointSecond.SetActive(true);
            SecondWaypoint.gameObject.SetActive(true);

        }
    }
    void Checkpoint_Second()
    {
        if (SecondCheckpoint.updateObjective == true)
        {
            MissionEnded = true;
            TextWaypointSecond.SetActive(false);
            SecondWaypoint.gameObject.SetActive(false);

        }
    }
   
}
