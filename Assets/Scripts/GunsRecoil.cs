using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunsRecoil : MonoBehaviour
{
    Vector3 currentRotation, targetRotation;

    [SerializeField] float snapiness, returnSpeed;
        

    // Update is called once per frame
    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snapiness * Time.deltaTime);

        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void RecoilFire(Vector3 recoil)
    {

        //Debug.Log("Recoiling");

        targetRotation += new Vector3(recoil.x, Random.Range(-recoil.y, recoil.y), Random.Range(-recoil.z, recoil.z));

    }

}
