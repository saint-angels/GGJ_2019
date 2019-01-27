using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : DestroyableObject
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lifeTime = 5f;

    protected override void OnHealthDepleted()
    {
        Destroy(gameObject);
    }

    protected override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed, Space.Self);
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }


}
