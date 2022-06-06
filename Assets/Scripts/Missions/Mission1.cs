using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission1 : MonoBehaviour
{
    [Header("Basics")]
    [SerializeField] Transform Player;
    [SerializeField] Vector3 InicialPosition;
    public bool MissionEnded;
    [Header("Waypoints")]
    [SerializeField] Transform FristWaypoint;
    [SerializeField] Transform SecondWaypoint;
    [Header("Text Waypoints")]
    [SerializeField] GameObject TextWaypointFrist;
    [SerializeField] GameObject TextWaypointSecond;

    //----------
    WayPointMission FristCheckpoint,SecondCheckpoint;
    

    private void Start()
    {
        Player.localPosition = InicialPosition;
        FristCheckpoint = FristWaypoint.GetComponent<WayPointMission>();
        SecondCheckpoint = SecondWaypoint.GetComponent<WayPointMission>();

    }

    void Update()
    {
        CheckPoint_One();
        CheckPoint_Second();
    }

    void CheckPoint_One()
    {
        if (FristCheckpoint.updateObjective == true)
        {
            TextWaypointFrist.SetActive(false);
            TextWaypointSecond.SetActive(true);
            FristWaypoint.gameObject.SetActive(false);
            SecondWaypoint.gameObject.SetActive(true);
        }
    }
    void CheckPoint_Second()
    {
        if (SecondCheckpoint.updateObjective == true)
        {
            TextWaypointSecond.SetActive(false);
            SecondWaypoint.gameObject.SetActive(false);
            MissionEnded = true;
        }
    }
}
