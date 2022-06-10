using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{

    [SerializeField] private GameObject zombiePreFab;
    [SerializeField] private GameObject zombieParent;

    [SerializeField] private Transform playerTransform;

    [SerializeField] private List<SpawnArea> spawnAreaList;

    [SerializeField] private float securityDistanceToSpawn;
    [SerializeField] float spawnCooldown;

    ObjectPooler pooler;
    

    //[System.Serializable]
    //public class SpawnPoint
    //{

    //    public GameObject spawnPoint;
        
    //    public float spawnRadius;
    //    //public float spawnRate;
    //    public float spawnCooldown;
        
    //    public int zombiesToSpawn;
    //    public List<GameObject> zombiesSpawned;

    //}

    //public List<SpawnPoint> spawnPoints;
    


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


        pooler = ObjectPooler.instance;

        SpawnZombie();



    }
    void Update()
    {

        spawnTimer += Time.deltaTime;
        //Debug.Log("Timer : " + spawnTimer);
        if (spawnTimer >= spawnCooldown)
        {
            //Debug.Log("Spawning");

            spawnTimer = 0f;
            SpawnZombie();

        }

        foreach (SpawnArea area in spawnAreaList)
        {

            for (int i = 0; i < area.zombiesList.Count; i++)
            {
                if (area.zombiesList[i] == null)
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

        for (int i = 0; i < spawnAreaList.Count; i++)
        {

            SpawnArea spawnArea = spawnAreaList[i];

            Vector3 positionToSpawn = Vector3.zero;
            Quaternion rotationToSpawn = Quaternion.identity;

            bool hasSpawned = false;

            int zombiesToSpawn = spawnArea.nZombiesSpawning - spawnArea.zombiesList.Count;

            for (int j = 0; j < zombiesToSpawn; j++)
            {

                //Debug.Log("Num : " + j);

                while (!hasSpawned)
                {
                    float min, max;

                    Vector3 position = spawnArea.SpawnCenter.transform.position;

                    min = position.x - spawnArea.SpawnRadius;
                    max = position.x + spawnArea.SpawnRadius;

                    float tempX = ((float)rnd.NextDouble() * (max - min) + min);
                    //Debug.Log("X : " + tempX);

                    min = position.z - spawnArea.SpawnRadius;
                    max = position.z + spawnArea.SpawnRadius;

                    float tempZ = ((float)rnd.NextDouble() * (max - min) + min);

                    //Debug.Log("Z : " + tempZ);  
                    float tempY = position.y;

                    positionToSpawn = new Vector3(tempX, tempY, tempZ);
                    rotationToSpawn = CalculateRotation();

                    positionToSpawn = GetPositionOnNavMesh(positionToSpawn);

                    if (CanSpawn(positionToSpawn))
                    {
                        hasSpawned = true;

                    }

                }

                if (positionToSpawn != Vector3.zero && Vector3.Distance(positionToSpawn, playerTransform.position) > 15f)
                {
                    //Debug.Log("Spawning : " + j);
                    //GameObject tempZombieGameObject = Instantiate(zombiePreFab, positionToSpawn, Quaternion.identity, zombieParent.transform);
                    //spawnArea.zombiesList.Add(tempZombieGameObject);

                    pooler.SpawnFromPool("Zombie", positionToSpawn, rotationToSpawn);

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

        if (Physics.Raycast(positionToSpawn, new Vector3(0, -1, 0), out hitInfo))
        {

            if (hitInfo.transform.tag == "Zombie")
            {

                canSpawn = false;

            }

        }

        return canSpawn;

    }


    public Vector3 GetPositionOnNavMesh (Vector3 position)
    {


        NavMeshHit hit;
        float range = 2.0f;

        if (NavMesh.SamplePosition(position, out hit, 2.0f, NavMesh.AllAreas))
        {

            position = hit.position;

        }


        return position;
    }

    public Vector3 CalculatePosition(Vector3 spawnPoint, float radius)
    {
        Vector3 position = Vector3.zero;

        float randomValue;
        Vector3 tempPosition = spawnPoint;

        randomValue = rnd.Next((int)-radius, (int) radius);

        tempPosition.x += randomValue;
        randomValue = rnd.Next((int)-radius, (int)radius);

        tempPosition.z += randomValue;

        Debug.Log("Temp Position : " + tempPosition);

        //if (CanSpawn(tempPosition) != tempPosition)
        //{
        //    Debug.Log("Can Spawn");
        //    position = tempPosition;
        //}

        return position;

    }


    public Quaternion CalculateRotation()
    {
        Quaternion rotation = Quaternion.identity;

        float randomRotationY = rnd.Next(0, 360);

        rotation = Quaternion.Euler(0f, randomRotationY, 0f);

        return rotation;
    }

  


    
}
