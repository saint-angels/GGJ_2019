using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomShell : SingletonComponent<RoomShell>
{
    [SerializeField] private GameObject[] roomPrefabs;

    GameObject currentRoom;

    public void GenerateNewRoom()
    {
        if (currentRoom != null)
        {
            Destroy(currentRoom);
        }

        var randomRoomPrefab = roomPrefabs[Random.Range(0, roomPrefabs.Length)];
        currentRoom = Instantiate(randomRoomPrefab, transform);
    }

    void Start()
    {
        GenerateNewRoom();    
    }


    void Update()
    {
        
    }
}
