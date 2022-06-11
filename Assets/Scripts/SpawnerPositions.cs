using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerPositions : MonoBehaviour
{


    [SerializeField] GameObject SpawnersParent;


    public static SpawnerPositions instance;

    List<GameObject> spawners;
    public List<SpawnPoints> spawnPoints;


    System.Random rnd;


    public List<SpawnPoints> SpawnPointsList
    {
        get { return spawnPoints; }
    }


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        rnd = new System.Random();

        spawners = new List<GameObject>();
        spawnPoints = new List<SpawnPoints>();

        foreach(Transform spawner in SpawnersParent.GetComponentInChildren<Transform>())
        {


            if (spawner.gameObject.name.Contains("Spawn"))
            {
                Debug.Log($"Spawner name : {spawner.gameObject.name}.");
                spawners.Add(spawner.gameObject);
            }


        }

        foreach(GameObject spawner in spawners)
        {

            SpawnPoints tempSpawnPoint = new SpawnPoints();


            tempSpawnPoint.spawnPoint = spawner;
            tempSpawnPoint.spawnRadius = GetRandomRadius();
            tempSpawnPoint.zombiesToSpawn = GetRandomNumberOfZombies();

            tempSpawnPoint.zombiesSpawned = new List<GameObject>();

            spawnPoints.Add(tempSpawnPoint);

        }


    }

    public float GetRandomRadius()
    {
        float radius = 0f;

        radius = rnd.Next(3, 8) + (float) rnd.NextDouble();

        return radius;

    }

    public int GetRandomNumberOfZombies()
    {
        int nZombies = 0;


        nZombies = rnd.Next(3, 7);

        return nZombies;
    }


}
