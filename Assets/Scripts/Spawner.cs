using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField] private GameObject zombiePreFab;
    [SerializeField] private GameObject zombieParent;

    [SerializeField] private Transform playerTransform;

    [SerializeField] private List<SpawnArea> spawnAreaList;

    [SerializeField] private float securityDistanceToSpawn;
    [SerializeField] float spawnCooldown;


    System.Random rnd;

    float spawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        rnd = new System.Random();

        spawnTimer = 0f;

        for(int i = 0; i < spawnAreaList.Count; i++)
        {

            spawnAreaList[i].zombiesList.Clear();

        }


        SpawnZombie();


    }

    // Update is called once per frame
    void Update()
    {

        spawnTimer += Time.deltaTime;
        //Debug.Log("Timer : " + spawnTimer);
        if(spawnTimer >= spawnCooldown)
        {
            //Debug.Log("Spawning");

            spawnTimer = 0f;
            SpawnZombie();

        }

        foreach(SpawnArea area in spawnAreaList)
        {

            for(int i = 0; i < area.zombiesList.Count; i++)
            {
                if(area.zombiesList[i] == null)
                {
                    area.zombiesList.RemoveAt(i);
                }
                //else
                //{

                //    Debug.Log("Zombie : " + i);
                //    Debug.Log("Position : " + area.zombiesList[i].transform.position);

                //}
            }

        }

    }

    public void SpawnZombie()
    {

        for(int i = 0; i < spawnAreaList.Count; i++)
        {

            SpawnArea spawnArea = spawnAreaList[i];

            Vector3 positionToSpawn = Vector3.zero;
            bool hasSpawned = false;

            int zombiesToSpawn = spawnArea.nZombiesSpawning - spawnArea.zombiesList.Count;

            for (int j = 0; j < zombiesToSpawn; j++)
            {

                //Debug.Log("Num : " + j);
    
                while (!hasSpawned)
                {
                    float min, max;

                    min = spawnArea.SpawnCenter.x - spawnArea.SpawnRadius;
                    max = spawnArea.SpawnCenter.x + spawnArea.SpawnRadius;

                    float tempX = ((float)rnd.NextDouble() * (max - min) + min);
                    //Debug.Log("X : " + tempX);

                    min = spawnArea.SpawnCenter.z - spawnArea.SpawnRadius;
                    max = spawnArea.SpawnCenter.z + spawnArea.SpawnRadius;

                    float tempZ = ((float)rnd.NextDouble() * (max - min) + min);

                    //Debug.Log("Z : " + tempZ);  
                    float tempY = spawnArea.SpawnCenter.y;

                    positionToSpawn = new Vector3(tempX, tempY, tempZ);

                    if (CanSpawn(positionToSpawn))
                    {                        
                        hasSpawned = true;

                    }

                }

                if(positionToSpawn != Vector3.zero && Vector3.Distance(positionToSpawn, playerTransform.position) > 15f)
                {
                    //Debug.Log("Spawning : " + j);
                    GameObject tempZombieGameObject = Instantiate(zombiePreFab, positionToSpawn, Quaternion.identity, zombieParent.transform);
                    spawnArea.zombiesList.Add(tempZombieGameObject);
                }
                else
                {
                    continue;
                }

                hasSpawned = false;

            }

        }

    }

    private bool CanSpawn(Vector3 positionToSpawn)
    {

        bool canSpawn = true;
        RaycastHit hitInfo;

        positionToSpawn.y += 2f;

        if(Physics.Raycast(positionToSpawn, new Vector3(0, -1, 0), out hitInfo))
        {

            if (hitInfo.transform.tag == "Zombie")
            {

                canSpawn = false;

            }

        }

        return canSpawn;

    }

    
}
