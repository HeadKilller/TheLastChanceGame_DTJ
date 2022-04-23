using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SpawnArea", menuName = "SpawnArea")]

public class SpawnArea : ScriptableObject
{

    public Vector3 SpawnCenter;
    public float SpawnRadius;

    public int nZombiesSpawning;
    public List<GameObject> zombiesList;


}
