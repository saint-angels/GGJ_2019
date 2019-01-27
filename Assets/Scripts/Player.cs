using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : SingletonComponent<Player>
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
    [SerializeField] private PlayerBulletTrail bulletTrailPrefab;
    [SerializeField] private Animator animator;
    [SerializeField] private float rechargeSpeed = 10f;
    [SerializeField] private float drainSpeed = 20f;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject particles;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color chargedColor;

    private float chargeLevel = 100;
    private bool aiming = false;

    public Vector2 Position2 { get { return new Vector2(transform.position.x, transform.position.y); } }

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

            if (lockNumber != 0)
            {
                AudioManager.Instance.PlayClip(true, lockNumber);
            }
        }
    }

    private const int maxLocks = 6;
    private int lockNumber = 0;
    private Dictionary<DestroyableObject, Target> enemyLocks = new Dictionary<DestroyableObject, Target>();
    private bool shooting;


    public void Die()
    {
        spriteRenderer.gameObject.SetActive(false);
        particles.gameObject.SetActive(true);

        RoomShell.Instance.GameOver();
    }

    public void Init()
    {
        LockNumber = 0;
        SetAimingState(false);
        chargeLevel = 100f;
        spriteRenderer.gameObject.SetActive(true);
        particles.gameObject.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (RoomShell.Instance.gameOver)
        {
            return;
        }

        //Movement
        float translationY = Input.GetAxisRaw("Vertical");
        float translationX = Input.GetAxisRaw("Horizontal");
        if (translationY != 0 || translationX != 0)
        {
            Vector3 translation = new Vector3(translationX, translationY, 0);
            transform.Translate(translation.normalized * Time.deltaTime * movementSpeed);
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
        float clampedX = Mathf.Clamp(transform.position.x, -4.2f, 4.2f);
        float clampedY = Mathf.Clamp(transform.position.y, -4.2f, 4.2f);
        transform.position = new Vector3(clampedX, clampedY, 0);


        //Aiming
        if (Input.GetButtonDown("Aim"))
        {
            if (!shooting && chargeLevel == 100)
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

        if (aiming)
        {
            chargeLevel -= drainSpeed * Time.deltaTime;
            if (chargeLevel <= 0 && !shooting)
            {
                SetAimingState(false);
                StartCoroutine(Shoot());
            }
        }
        else if (!aiming && !shooting)
        {
            chargeLevel += rechargeSpeed * Time.deltaTime;
        }
        chargeLevel = chargeLevel < 0f ? 0f : chargeLevel;
        chargeLevel = chargeLevel > 100f ? 100f : chargeLevel;
        spriteRenderer.color = Mathf.Approximately(chargeLevel, 100f) ? chargedColor : normalColor;
        RoomShell.Instance.SetChargeLevel(chargeLevel);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!shooting && LockNumber <= maxLocks)
        {
            var dObject = collision.GetComponentInParent<DestroyableObject>();
            if (dObject != null)
            {
                if (enemyLocks.ContainsKey(dObject))
                {
                    bool hpLeft = dObject.Health > enemyLocks[dObject].shots;
                    bool lastLockLongAgo = Time.time > enemyLockCooldown + enemyLocks[dObject].lastLocktime;
                    if (hpLeft && lastLockLongAgo)
                    {
                        Target updatedTarget = new Target(enemyLocks[dObject].shots + 1, Time.time);
                        enemyLocks[dObject] = updatedTarget;
                        LockNumber++;
                        transform.position += Vector3.right * Random.Range(-.1f, 1f) * Time.deltaTime;
                    }
                }
                else
                {
                    Target newTarget = new Target(1, Time.time);
                    enemyLocks.Add(dObject, newTarget);
                    dObject.SetLockedLabelVisible(true);
                    LockNumber++;
                    transform.position += Vector3.right * Random.Range(-.1f, 1f) * Time.deltaTime;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var enemy = collision.collider.GetComponent<EnemyBase>();
        if (enemy!= null)
        {
            Die();
        }
        else
        {
            var bullet = collision.collider.GetComponent<EnemyBullet>();
            if (bullet != null)
            {
                Die();
            }
        }

    }

    IEnumerator Shoot()
    {
        shooting = true;
        yield return new WaitForSeconds(.1f);
        int shotTotal = 0;
        foreach (var enemyLockPair in enemyLocks)
        {
            DestroyableObject enemy = enemyLockPair.Key;
            for (int shotIdx = 0; shotIdx < enemyLockPair.Value.shots; shotIdx++)
            {
                if (enemy != null)
                {
                    var newTrailPrefab = Instantiate(bulletTrailPrefab, transform.position, Quaternion.identity);
                    newTrailPrefab.Init(enemy.transform);

                    enemy.Damage();
                    AudioManager.Instance.PlayClip(false, shotTotal);

                    bool lastShot = shotIdx + 1 == enemyLockPair.Value.shots;
                    if (lastShot)
                    {
                        enemy.SetLockedLabelVisible(false);
                    }
                    else
                    {
                        yield return new WaitForSeconds(.05f);
                    }
                    shotTotal++;
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
        lockNumberLabel.enabled = isAiming;
        crosshairs.SetActive(isAiming);
        body.SetActive(!isAiming);
        aiming = isAiming;
    }
}
