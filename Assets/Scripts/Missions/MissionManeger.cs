using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManeger : MonoBehaviour
{
    [SerializeField] Mission1 m1;
    [SerializeField] Mission2 m2;

    void Update()
    {
        if(m1.MissionEnded)
        {
            m1.gameObject.SetActive(false);
            m2.gameObject.SetActive(true);
        }
        
    }
}
