using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSenses : MonoBehaviour
{
    [SerializeField] private GameObject zombieGameObject;

    ZombieBehavior zombieBehavior;

    bool isSensingPlayer;

    // Start is called before the first frame update
    void Start()
    {
        isSensingPlayer = false;   
        zombieBehavior = zombieGameObject.GetComponent<ZombieBehavior>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isSensingPlayer)
        {

            zombieBehavior.ZombieSensing();

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.tag == "Player")
        {
            isSensingPlayer = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isSensingPlayer = false;
        }
    }
}
