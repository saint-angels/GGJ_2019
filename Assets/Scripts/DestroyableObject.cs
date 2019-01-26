using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DestroyableObject : MonoBehaviour
{
    [SerializeField] private int health = 1;
    [SerializeField] private GameObject lockedLabel;

    public int Health { get { return health; } }

    protected virtual void Start()
    {
        SetLockedLabelVisible(false);
    }

    public virtual void SetLockedLabelVisible(bool isVisible)
    {
        lockedLabel.SetActive(isVisible);
    }

    public virtual void Damage()
    {
        health -= 1;

        if (health <= 0)
        {
            OnHealthDepleted();
        }
    }

    protected abstract void OnHealthDepleted();
}
