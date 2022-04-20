using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField] private GameObject zombiePreFab;
    [SerializeField] private GameObject zombieParent;

    [SerializeField] private int spawnRadius;

    List<GameObject> ZombiesList;

    System.Random rnd;
    // Start is called before the first frame update
    void Start()
    {
        ZombiesList = new List<GameObject>();
        rnd = new System.Random();

        SpawnZombie();
    }

    // Update is called once per frame
    void Update()
    {
        if(ZombiesList.Count <= 0)
        {
            SpawnZombie();
        }
    }

    public void SpawnZombie()
    {

        for(int i = 0; i < 5; i++)
        {
            float tempX = transform.position.x + rnd.Next(-spawnRadius, spawnRadius);
            float tempZ = transform.position.y + rnd.Next(-spawnRadius, spawnRadius);

            Vector3 tempPosition = new Vector3(tempX, transform.position.y, tempZ);

            GameObject tempObject = (GameObject)Instantiate(zombiePreFab, tempPosition, Quaternion.identity, zombieParent.transform);
            ZombiesList.Add(tempObject);

        }

    }

    
}
