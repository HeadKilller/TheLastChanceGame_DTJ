using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseAutoCheck : MonoBehaviour
{
    public bool canClose;
    [SerializeField] Inventory inv;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            canClose = true;
            this.gameObject.SetActive(false);
            if(!inv.GetComponent<Inventory>().MaskEquiped)
            {
                Player.instance.Damage(100f);
            }

        }
    }
}
