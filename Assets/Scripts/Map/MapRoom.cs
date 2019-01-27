using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRoom : MonoBehaviour
{
    public int Id { get; private set; }

    private int gridSize = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(int id)
    {
        this.Id = id;
    }

    public bool Overlapping(List<MapRoom> rooms)
    {
        foreach (var otherRoom in rooms)
        {
            if (otherRoom.Id == Id)
            {
                continue;
            }

            bool overlapping = otherRoom.transform.position == transform.position;
            bool idCheck = Id > otherRoom.Id;
            if (overlapping && idCheck)
            {
                bool moveX = Random.value > .5f;
                float translateAmount = gridSize * (Random.value > .5f ? 1f : -1f);
                Vector3 translation;
                if (moveX)
                {
                    translation = new Vector3(translateAmount, 0, 0);
                }
                else
                {
                    translation = new Vector3(0, translateAmount, 0);
                }
                transform.Translate(translation);
            }
        }


        foreach (var otherRoom in rooms)
        {
            if (otherRoom.Id == Id)
            {
                continue;
            }

            bool overlapping = otherRoom.transform.position == transform.position;
            if (overlapping)
            {
                return true;
            }
        }
        return false;
    }
}
