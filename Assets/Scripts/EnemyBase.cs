using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private GameObject lockedLabel;

    public int Health { get { return health; } }


    // Start is called before the first frame update
    void Start()
    {
        SetLockedLabelVisible(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Random.insideUnitCircle * movementSpeed * Time.deltaTime);
    }

    public void Damage()
    {
        health -= 1;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetLockedLabelVisible(bool isVisible)
    {
        lockedLabel.SetActive(isVisible);
    }
}
