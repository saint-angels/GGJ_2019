using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomShell : SingletonComponent<RoomShell>
{
    public Color attackableColor;

    [SerializeField] private Room[] roomPrefabs;
    [SerializeField] private EnemyBase[] enemyPrefabs;

    Room currentRoom;

    public void GenerateNewRoom()
    {
        if (currentRoom != null)
        {
            Destroy(currentRoom.gameObject);
        }

        var randomRoomPrefab = roomPrefabs[Random.Range(0, roomPrefabs.Length)];
        currentRoom = Instantiate(randomRoomPrefab, transform);
        Player.Instance.transform.position = currentRoom.playerSpawnPoint.position;
    }

    public EnemyBase SpawnRandomEnemy(Vector3 atPoint)
    {
        int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);
        EnemyBase selectedPrefab = enemyPrefabs[0];
        var newEnemy = Instantiate(selectedPrefab, atPoint, Quaternion.identity);

        return newEnemy;
    }

    void Start()
    {
        GenerateNewRoom();    
    }


    void Update()
    {
        
    }
}
