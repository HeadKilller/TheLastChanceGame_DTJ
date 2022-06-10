using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectPooler : MonoBehaviour
{

    [System.Serializable]
    public class Pool
    {
 
        public string tag;
        public GameObject prefab;
        public int size;

        public GameObject zombieParent;

    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;


    [Header("Singleton")]
    public static ObjectPooler instance;


    // Start is called before the first frame update
    void Awake()
    {

        instance = this;

        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in pools)
        {

            Queue<GameObject> objectPool = new Queue<GameObject>();

            for(int i = 0; i < pool.size; i++)
            {

                GameObject obj = Instantiate(pool.prefab, pool.zombieParent.transform);

                obj.SetActive(false);
                objectPool.Enqueue(obj);

            }

            poolDictionary.Add(pool.tag, objectPool);

        }


    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {

        if(tag == null)
        {
            Debug.LogWarning($"Tag {tag} is null.");
            return null;

        }


        if (!poolDictionary.ContainsKey(tag))
        {

            Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
            return null;

        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.GetComponent<NavMeshAgent>().Warp(position);
        //objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;

    }

    public void DestroyToPool(GameObject objectToDestroy)
    {

        objectToDestroy.SetActive(false);

    }



}
