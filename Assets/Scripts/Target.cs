using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{

    private Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }
    
    private void OnMouseOver()
    {
        if (renderer != null)
        {

            renderer.material.color = new Color(171f, 171f, 171f, 0.2f);            

        }
    }

    private void OnMouseExit()
    {

        if (renderer != null)
        {

            renderer.material.color = Color.white;

        }

    }
}
