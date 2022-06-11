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
    SpawnerPositions spawnerPositions;

    private List<SpawnPoints> spawnPointsList;

    //public List<SpawnPoint> spawnPoints;



    System.Random rnd;

    float spawnTimer;
    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {

        spawnerPositions = SpawnerPositions.instance;
        pooler = ObjectPooler.instance;

        rnd = new System.Random();

        spawnTimer = 0f;

        spawnPointsList = spawnerPositions.spawnPoints;

        Debug.Log($"Spawn Points List Size : {spawnPointsList}.");

        for(int i = 0; i < spawnPointsList.Count; i++)
        {

            try
            {
                spawnPointsList[i].zombiesSpawned.Clear();
            }
            catch(System.Exception e)
            {
                Debug.LogError($"Error : {e.Message}.");
            }

        }



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

        foreach (SpawnPoints point in spawnPointsList)
        {
            if (point.zombiesSpawned == null)
                continue;

            for (int i = 0; i < point.zombiesSpawned.Count; i++)
            {
                if (point.zombiesSpawned[i] == null)
                {
                    point.zombiesSpawned.RemoveAt(i);
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

        for (int i = 0; i < spawnPointsList.Count; i++)
        {

            SpawnPoints spawnPoint = spawnPointsList[i];

            Vector3 positionToSpawn = Vector3.zero;
            Quaternion rotationToSpawn = Quaternion.identity;

            bool hasSpawned = false;
            int zombiesToSpawn;

            try
            {
                zombiesToSpawn = spawnPoint.zombiesToSpawn - spawnPoint.zombiesSpawned.Count;
            }
            catch
            {
                zombiesToSpawn = spawnPoint.zombiesToSpawn;
            }

            for (int j = 0; j < zombiesToSpawn; j++)
            {

                //Debug.Log("Num : " + j);

                while (!hasSpawned)
                {
                    float min, max;

                    Vector3 position = spawnPoint.spawnPoint.transform.position;

                    min = position.x - spawnPoint.spawnRadius;
                    max = position.x + spawnPoint.spawnRadius;

                    float tempX = ((float)rnd.NextDouble() * (max - min) + min);
                    //Debug.Log("X : " + tempX);

                    min = position.z - spawnPoint.spawnRadius;
                    max = position.z + spawnPoint.spawnRadius;

                    float tempZ = ((float)rnd.NextDouble() * (max - min) + min);

                    //Debug.Log("Z : " + tempZ);  
                    float tempY = position.y;

                    positionToSpawn = new Vector3(tempX, tempY, tempZ);

                    positionToSpawn = GetPositionOnNavMesh(positionToSpawn);

                    if (CanSpawn(positionToSpawn))
                    {
                        hasSpawned = true;

                    }

                }

                rotationToSpawn = CalculateRotation();
                

                if (positionToSpawn != Vector3.zero && Vector3.Distance(positionToSpawn, playerTransform.position) > 15f)
                {
                    //Debug.Log("Spawning : " + j);
                    //GameObject tempZombieGameObject = Instantiate(zombiePreFab, positionToSpawn, Quaternion.identity, zombieParent.transform);
                    //spawnArea.zombiesList.Add(tempZombieGameObject);

                    //Debug.Log($"Position is {positionToSpawn}.");
                    //Debug.Log($"Rotation is {rotationToSpawn}.");

                    Debug.Log($"Pooler : {pooler}.");
                    GameObject tempobj = pooler.SpawnFromPool("Zombie", positionToSpawn, rotationToSpawn);

                    spawnPointsList[i].zombiesSpawned.Add(tempobj);

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

        //positionToSpawn.y -= 1f;

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
            //Debug.Log($"Hit Position : {hit.position}.");
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
public struct SpawnPoints
{

    public GameObject spawnPoint;

    public float spawnRadius;

    public int zombiesToSpawn;
    public List<GameObject> zombiesSpawned;

}
