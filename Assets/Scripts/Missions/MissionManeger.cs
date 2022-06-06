using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManeger : MonoBehaviour
{
    [SerializeField] GameObject m1;
    [SerializeField] GameObject m2;
    [SerializeField] GameObject m3;


    void Update()
    {
        if(m1.GetComponent<Mission1>().MissionEnded)
        {
            m1.SetActive(false);
            m2.SetActive(true);
        }
        if (m2.GetComponent<Mission2>().MissionEnded)
        {
            m2.SetActive(false);
            m3.SetActive(true);
        }

    }
}
