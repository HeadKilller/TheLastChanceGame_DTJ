using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseAutoCheck : MonoBehaviour
{
    public bool canClose;
    [SerializeField] Inventory inv;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "PlayerCharacterController")
        {
            canClose = true;
            this.gameObject.SetActive(false);
            if(!inv.GetComponent<Inventory>().MaskEquiped)
            {
                other.GetComponent<Player>().health = 0;
            }

        }
    }
}
