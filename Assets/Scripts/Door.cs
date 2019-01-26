using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : DestroyableObject
{
    [SerializeField] private GameObject lockedState;
    [SerializeField] private GameObject unlockedState;

    protected override void OnHealthDepleted()
    {
        SetLocked(false);
    }

    protected override void Start()
    {
        base.Start();
        SetLocked(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetLocked(bool isLocked)
    {
        lockedState.SetActive(isLocked);
        unlockedState.SetActive(!isLocked);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player != null)
        {
            RoomShell.Instance.GenerateNewRoom();
        }
    }
}
