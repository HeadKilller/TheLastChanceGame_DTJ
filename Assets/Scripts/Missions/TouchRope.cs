using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchRope : MonoBehaviour
{
    public bool canEnd;
    private void OnTriggerEnter(Collider other)
    {  
        if (other.transform.name == "PlayerCharacterController")
        {
            canEnd = true;
        }
    }
}
