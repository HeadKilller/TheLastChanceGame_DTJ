using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlacement : MonoBehaviour
{
    public Transform TrapSpawnAndHeldLocation;
    public GameObject trapToBePlaced;
    bool placingTrapDown = false;
    bool meshAndRigidbodyDeactivationComplete = false;

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) && placingTrapDown == false)
        {
            SpawnTrapInFrontOfPlayer();
            placingTrapDown = true;
        }
        if(placingTrapDown == true)
        {
            MoveTrapInFrontOfPlayerUntilInput();
        }
    }

    void SpawnTrapInFrontOfPlayer()
    {
        Instantiate(trapToBePlaced, TrapSpawnAndHeldLocation.transform.position,
                    trapToBePlaced.transform.rotation);
    }

    void MoveTrapInFrontOfPlayerUntilInput()
    {
        if (meshAndRigidbodyDeactivationComplete == false)
            TrapMeshAndGravityDeactivation();

        TrapGroundCollisionCheck();
        trapToBePlaced.transform.position = TrapSpawnAndHeldLocation.transform.position;

        if (Input.GetKeyDown(KeyCode.DownArrow) && meshAndRigidbodyDeactivationComplete == true)
            TrapMeshAndGravityReactivation();
    }

    void TrapMeshAndGravityDeactivation()
    {
        trapToBePlaced.GetComponent<Rigidbody>().useGravity = false;
        trapToBePlaced.GetComponent<MeshCollider>().enabled = false;
        meshAndRigidbodyDeactivationComplete = true;
    }

    void TrapMeshAndGravityReactivation()
    {
        trapToBePlaced.GetComponent<Rigidbody>().useGravity = true;
        trapToBePlaced.GetComponent<MeshCollider>().enabled = true;
        meshAndRigidbodyDeactivationComplete = false;
    }

    void TrapGroundCollisionCheck()
    {
        OnTriggerEnter(trapToBePlaced.GetComponentInChildren<MeshCollider>());
    }

    private void OnTriggerEnter(Collider other)
    {
        trapToBePlaced.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
    }

}
