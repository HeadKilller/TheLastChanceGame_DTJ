using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointMission : MonoBehaviour
{
    public bool updateObjective;
    private void OnTriggerEnter(Collider other)
    {
       
        if(other.transform.name == "Player")
        {
            updateObjective = true;
        }

    }
}
