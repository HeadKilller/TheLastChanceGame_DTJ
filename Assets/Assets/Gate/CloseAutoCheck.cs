using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseAutoCheck : MonoBehaviour
{
    public bool canClose;

    private void OnCollisionEnter(Collision collision)
    { 
        if (collision.transform.name == "Player")
        {
            canClose = true;
            this.gameObject.SetActive(false);
           
        }
    }
}
