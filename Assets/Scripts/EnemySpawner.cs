using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    void Awake()
    {
        RoomShell.Instance.SpawnRandomEnemy(transform.position);       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
