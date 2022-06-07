using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioFixed : MonoBehaviour
{
    public bool RadioFix;


    public static RadioFixed radio;

    private void Start()
    {
        radio = this;
    }

    public void FinishGame()
    {

        Debug.Log("CONGRATULATIONS. YOU WIN.");

    }

}
