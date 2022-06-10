using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SpawnArea", menuName = "SpawnArea")]

public class SpawnArea : ScriptableObject
{

    public GameObject SpawnCenter;
    public float SpawnRadius;

    public int nZombiesSpawning;
    public List<GameObject> zombiesList;


}
