using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGrounded : MonoBehaviour
{
    LayerMask playerLayer;

    private bool isGrounded;

    public bool IsGrounded
    {
        get { return isGrounded; }
    }

    // Start is called before the first frame update
    void Start()
    {
        isGrounded = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
        

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != playerLayer)
        {
            Debug.Log(other.name);
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != playerLayer)
        {
            Debug.Log(other.name);
            isGrounded = false;
        }
    }
}
