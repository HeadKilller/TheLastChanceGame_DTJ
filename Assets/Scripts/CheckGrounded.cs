using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGrounded : MonoBehaviour
{
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask postProcessingLayer;

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
        if (other.gameObject.layer != playerLayer && other.gameObject.layer != postProcessingLayer && other.name != "Post-Processing")
        {
            Debug.Log("Stay: " + other.name);

            isGrounded = true;
            Debug.Log(IsGrounded);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != playerLayer && other.gameObject.layer != postProcessingLayer && other.tag != "Post-Processing")
        {
            Debug.Log("Exit: " + other.name);

            isGrounded = false;
            Debug.Log(IsGrounded);
        }
    }
}
