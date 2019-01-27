using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : DestroyableObject
{
    public event Action OnDeath = () => { };

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

    protected override void OnHealthDepleted()
    {
        OnDeath();
    }

    public void Stop()
    {
        StopAllCoroutines();
        agent.enabled = false;
    }

}