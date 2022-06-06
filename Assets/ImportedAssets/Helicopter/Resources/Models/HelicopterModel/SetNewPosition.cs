using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetNewPosition : MonoBehaviour
{
    public bool canMove;
    public Vector3 newPosition;
    void Update()
    {
        if(canMove)
        {
            this.transform.localPosition = newPosition;
            canMove = false;
        }
    }
}
