using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : DestroyableObject
{
    [SerializeField] private float movementSpeed = 5f;

    private PolyNavAgent agent;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        agent = GetComponent<PolyNavAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(Player.Instance.transform.position);
    }

    protected override void OnHealthDepleted()
    {
        Destroy(gameObject);
    }
}
