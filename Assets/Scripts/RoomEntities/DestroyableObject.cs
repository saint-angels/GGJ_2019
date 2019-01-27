using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public abstract class DestroyableObject : MonoBehaviour
{
    [SerializeField] private int health = 1;
    [SerializeField] private GameObject lockedLabel;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public int Health { get { return health; } }

    private const float damageShakeAmount = .1f;
    private bool isDead = false;
    private Shaker shaker;

    protected virtual void Start()
    {
        SetLockedLabelVisible(false);
        shaker = GetComponent<Shaker>();
        spriteRenderer.color = RoomShell.Instance.attackableColor;
    }

    public virtual void SetLockedLabelVisible(bool isVisible)
    {
        lockedLabel.SetActive(isVisible);
    }

    public virtual void Damage()
    {
        if (isDead == false)
        {
            health -= 1;
            shaker.Shake(.15f, damageShakeAmount);

            if (health <= 0)
            {
                isDead = true;
                Invoke("OnHealthDepleted", shaker.ShakeDuration);
            }
        }
    }

    protected abstract void OnHealthDepleted();
}
