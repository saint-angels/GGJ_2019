using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    struct Target
    {
        public Target(int shots, float lastLockTime)
        {
            this.shots = shots;
            this.lastLocktime = lastLockTime;
        }
        public int shots;
        //public EnemyBase enemy;
        public float lastLocktime;
    }

    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float enemyLockCooldown = .1f;
    [SerializeField] private TextMeshProUGUI lockNumberLabel;
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject crosshairs;

    private int LockNumber
    {
        get
        {
            return lockNumber;
        }
        set
        {
            lockNumber = value;
            switch (lockNumber)
            {
                case maxLocks:
                    lockNumberLabel.text = "MAX";
                    break;
                case 0:
                    lockNumberLabel.text = "+";
                    break;
                default:
                    lockNumberLabel.text = lockNumber.ToString();
                    break;
            }
        }
    }

    private const int maxLocks = 5;
    private int lockNumber = 0;
    private Dictionary<EnemyBase, Target> enemyLocks = new Dictionary<EnemyBase, Target>();
    private bool shooting;


    // Start is called before the first frame update
    void Start()
    {
        LockNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        float translationY = Input.GetAxisRaw("Vertical");
        float translationX = Input.GetAxisRaw("Horizontal");
        if (translationY != 0 || translationX != 0)
        {
            Vector3 translation = new Vector3(translationX, translationY, 0);
            transform.Translate(translation.normalized * Time.deltaTime * movementSpeed);
        }


        //Aiming
        if (Input.GetButtonDown("Aim"))
        {
            if (!shooting)
            {
                SetAimingState(true);
            }
        }
        else if (Input.GetButtonUp("Aim"))
        {
            if (!shooting)
            {
                SetAimingState(false);
                StartCoroutine(Shoot());
            }
        }        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!shooting && LockNumber < maxLocks)
        {
            var enemy = collision.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                if (enemyLocks.ContainsKey(enemy))
                {
                    bool hpLeft = enemy.Health > enemyLocks[enemy].shots;
                    bool lastLockLongAgo = Time.time > enemyLockCooldown + enemyLocks[enemy].lastLocktime;
                    if (hpLeft && lastLockLongAgo)
                    {
                        Target updatedTarget = new Target(enemyLocks[enemy].shots + 1, Time.time);
                        enemyLocks[enemy] = updatedTarget;
                        LockNumber++;
                    }
                }
                else
                {
                    Target newTarget = new Target(1, Time.time);
                    enemyLocks.Add(enemy, newTarget);
                    enemy.SetLockedLabelVisible(true);
                    LockNumber++;
                }
            }
        }
    }

    IEnumerator Shoot()
    {
        shooting = true;
        yield return new WaitForSeconds(.1f);
        foreach (var enemyLockPair in enemyLocks)
        {
            EnemyBase enemy = enemyLockPair.Key;
            for (int shotIdx = 0; shotIdx < enemyLockPair.Value.shots; shotIdx++)
            {
                if (enemy != null)
                {
                    enemy.Damage();
                }
                bool lastShot = shotIdx + 1 == enemyLockPair.Value.shots;
                if (lastShot)
                {
                    enemy.SetLockedLabelVisible(false);
                }
                else
                {
                    yield return new WaitForSeconds(.1f);
                }
            }

            yield return new WaitForSeconds(.1f);
        }

        enemyLocks.Clear();
        LockNumber = 0;

        shooting = false;
    }

    private void SetAimingState(bool isAiming)
    {
        crosshairs.SetActive(isAiming);
        body.SetActive(isAiming == false);
    }
}
