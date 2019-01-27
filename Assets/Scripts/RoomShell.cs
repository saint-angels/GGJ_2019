using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomShell : SingletonComponent<RoomShell>
{
    public Color attackableColor;

    [SerializeField] private Room[] roomPrefabs;

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

    void Start()
    {
        GenerateNewRoom();    
    }


    void Update()
    {
        
    }
}
