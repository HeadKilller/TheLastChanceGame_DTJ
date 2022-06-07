using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseAutoCheck : MonoBehaviour
{
    public bool canClose;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "PlayerCharacterController")
        {
            canClose = true;
            this.gameObject.SetActive(false);
            if(other.GetComponent<Inventory>().MaskEquiped)
            {
                other.GetComponent<Player>().health = 0;
            }

        }
    }
}
