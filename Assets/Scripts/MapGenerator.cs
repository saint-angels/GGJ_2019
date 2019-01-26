using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Room roomPrefab;
    [SerializeField] private int roomCount = 30;
    [SerializeField] bool shouldGenerate = true;
    List<Room> rooms = new List<Room>();

    void Start()
    {
        //Generate
        if (shouldGenerate)
        {
            for (int i = 0; i < roomCount; i++)
            {
                var newRoom = Instantiate(roomPrefab, transform);
                newRoom.Init(i);
                rooms.Add(newRoom);
            }

            StartCoroutine(DisperseRooms());
        }
    }

    IEnumerator DisperseRooms()
    {
        bool overlapExists;
        do
        {
            overlapExists = false;
            foreach (var room in rooms)
            {
                if (room.Overlapping(rooms))
                {
                    overlapExists = true;
                }
                yield return new WaitForSeconds(.01f);
            }
        } while (overlapExists);

        print("Overlapping eliminated!");
    }

    void Update()
    {
        
    }
}
