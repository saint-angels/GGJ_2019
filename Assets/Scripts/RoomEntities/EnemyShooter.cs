using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : EnemyBase
{
    [SerializeField] private float targetPlayerDistance = 1f;

    protected override void ChooseTargetPosition()
    {
        var ba = (transform.position - Player.Instance.transform.position).normalized;
        Vector3 destination = Player.Instance.transform.position + ba * targetPlayerDistance;
        agent.SetDestination(destination);
    }

    protected override void OnHealthDepleted()
    {
        Destroy(gameObject);
    }
}
