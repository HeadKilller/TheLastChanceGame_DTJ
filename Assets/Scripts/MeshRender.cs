using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRender : MonoBehaviour
{
    Transform mainCamTransform; // Stores the FPS camera transform
    private bool visible = true;
    public float distanceToAppear = 50;
    Renderer objRenderer;

    private void Start()
    {
        mainCamTransform = Camera.main.transform;//Get camera transform reference
        objRenderer = gameObject.GetComponent<Renderer>(); //Get render reference
    }
    private void Update()
    {
        disappearChecker();
    }
    private void disappearChecker()
    {
        float distance = Vector3.Distance(mainCamTransform.position, transform.position);

        // We have reached the distance to Enable Object
        if (distance < distanceToAppear)
        {
            if (!visible)
            {
                objRenderer.enabled = true; // Show Object
                visible = true;
                //Debug.Log("Visible");
            }
        }
        else if (visible)
        {
            objRenderer.enabled = false; // Hide Object
            visible = false;
           // Debug.Log("InVisible");
        }
    }
}
