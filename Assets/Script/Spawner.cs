using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Vector2Int spawnPoint;

    public GroupType[] types;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Vector2Int[] Spawn()
    {
        // Random Index
        int i = Random.Range(0, types.Length);

        Vector2Int[] blocks = (Vector2Int[])types[i].blocks.Clone();

        return blocks;
    }
}
