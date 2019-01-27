using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : EnemyBase
{
    [SerializeField] private float targetPlayerDistance = 1f;
    [SerializeField] private EnemyBullet bulletPrefab;
    [SerializeField] private float shotCooldown = 1f;

    protected override void ChooseTargetPosition()
    {
        var ba = (transform.position - Player.Instance.transform.position).normalized;
        Vector3 destination = Player.Instance.transform.position + ba * targetPlayerDistance;
        agent.SetDestination(destination);


        if (agent.hasPath == false)
        {
            var randomOffset = Random.insideUnitCircle;
            var randomOffset3 = new Vector3(randomOffset.x, randomOffset.y, 0);
            agent.SetDestination(transform.position + randomOffset3);
        }
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(ShootingCoroutine());
    }

    IEnumerator ShootingCoroutine()
    {
        while (true)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);
            bool closeEnoughToShoot = distanceToPlayer <= targetPlayerDistance + .5f;
            if (closeEnoughToShoot)
            {
                Vector3 vectorToTarget = Player.Instance.transform.position - transform.position;
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
                Quaternion newBulletRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                var newBullet = Instantiate(bulletPrefab, transform.position, newBulletRotation);
                
                yield return new WaitForSeconds(shotCooldown);
            }
            else
            {
                yield return null;
            }
        }
    }


    protected override void OnHealthDepleted()
    {
        base.OnHealthDepleted();
        Destroy(gameObject);
    }
}
