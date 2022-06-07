using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{

    private Outline outline;

    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();
    }
    
    private void OnMouseOver()
    {
        try
        {
            outline.enabled = true;
        }
        catch(System.Exception e)
        {
            Debug.LogError(e.Message);
        }

    }

    private void OnMouseExit()
    {

        try
        {
            outline.enabled = false;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }

    }
}
