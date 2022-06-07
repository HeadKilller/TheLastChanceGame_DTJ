using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidesWith : MonoBehaviour
{

    public float punchDamage = 20f;


    bool hasHitted;

    // Start is called before the first frame update
    void Start()
    {
        hasHitted = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player" && !hasHitted)
        {

            Player.instance.Damage(punchDamage);

            hasHitted = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {

        hasHitted = false;

    }

}
