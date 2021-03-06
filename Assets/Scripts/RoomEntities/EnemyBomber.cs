﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomber : EnemyBase
{
    [SerializeField] private float explosionRadius = 1f;
    [SerializeField] private float explosionDuration = .5f;
    [SerializeField] private Transform explosionPart;

    protected override void Start()
    {
        base.Start();
        explosionPart.gameObject.SetActive(false);
    }

    protected override void OnHealthDepleted()
    {
        base.OnHealthDepleted();

        //Blow up
        explosionPart.gameObject.SetActive(true);
        explosionPart.localScale = Vector3.one * explosionRadius;
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, explosionRadius, Vector2.zero);
        if (hit.collider != null)
        {
            var player = hit.collider.GetComponent<Player>();
            if (player != null)
            {
                
            }
        }
        Destroy(gameObject, explosionDuration);
    }

    protected override void ChooseTargetPosition()
    {
        agent.SetDestination(Player.Instance.Position2);
        if (agent.hasPath == false)
        {
            var randomOffset = Random.insideUnitCircle;
            var randomOffset3 = new Vector3(randomOffset.x, randomOffset.y, 0);
            agent.SetDestination(transform.position + randomOffset3);
        }
    }
}
