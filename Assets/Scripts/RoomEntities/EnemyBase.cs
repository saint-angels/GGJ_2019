using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : DestroyableObject
{
    [SerializeField] private float targetPositionSelectionCooldown = .5f;

    protected PolyNavAgent agent;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        agent = GetComponent<PolyNavAgent>();
        StartCoroutine(ChooseTargetPositionRoutine());
    }

    protected abstract void ChooseTargetPosition();

    IEnumerator ChooseTargetPositionRoutine()
    {
        while (true)
        {
            ChooseTargetPosition();
            yield return new WaitForSeconds(targetPositionSelectionCooldown);
        }
    }
}