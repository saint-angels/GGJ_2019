using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Can : DestroyableObject
{
    protected override void OnHealthDepleted()
    {
        Destroy(gameObject);
    }
}
