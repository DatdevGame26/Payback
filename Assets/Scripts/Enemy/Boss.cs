using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : Enemy
{
    [Header("Boss: All VFX")]
    [SerializeField] GameObject teleportVFX;
    [SerializeField] GameObject summonVFX;
    [SerializeField] GameObject shootGrenadeVFX;

    [Header("Boss: Shoot Missile")]
    [SerializeField] GameObject missilePrefab;
    [SerializeField] float missileSpeed;
    [SerializeField] Transform missileFirePoint;

    [Header("Boss: Shoot Grenade")]
    [SerializeField] GameObject grenadePrefab;
    [SerializeField] float grenadeSpeed;
    [SerializeField] Transform[] allGrenadeFirePoints;

    [Header("Boss: Teleport")]
    [SerializeField] float teleportRadius;
    [SerializeField] LayerMask avoidTeleportLayer;

    [Header("Boss: Super Bomb")]
    [SerializeField] GameObject superBombPrefab;

    FillBar healthBar;
    float agressiveMeter;
    float behaviourTimer;
    bool isBossStarted;

    protected override void Start()
    {
        base.Start();
        AudioManager.Instance.PlaySound("boss_summon_super_bomb", gameObject);
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        healthBar = GameObject.FindGameObjectWithTag("BossHealthBar").GetComponent<FillBar>();
        healthBar.transform.GetChild(0).gameObject.SetActive(true);

        StartCoroutine(WaitThenStartBoss(3));
        itemSpawner.setSpawnOffset(new Vector3(0, 8.5f, 0));
    }

    protected override void Update()
    {
        base.Update();
        if (isStopAction()) return;

        if (healthBar != null)
        {
            healthBar.updateFillBar(currentHealth, maxHealth);
        }

        if (isBossStarted)
        {
            BossBehaviour();
        }
    }

    void BossBehaviour()
    {
        behaviourTimer -= Time.deltaTime;

        if (behaviourTimer > 0) return;

        faceToPlayer();

        if (Random.value <= 0.2f)
        {
            playAnimationAndSetBehaviorTimer("boss_super_bomb", 5);
            return;
        }

        float chanceToTeleport = getDistanceToPlayer() <= 20 ||
            getDistanceToPlayer() >= 130 ? 1 : 0.3f;

        if (Random.value <= chanceToTeleport)
        {
            playAnimationAndSetBehaviorTimer("boss_teleport", 4);
        }
        else
        {
            if (Random.value <= 0.4f)
            {
                playAnimationAndSetBehaviorTimer("boss_shoot_missile", 5);

            }
            else
            {
                playAnimationAndSetBehaviorTimer("boss_shoot_grenade", 3);
            }
        }
    }

    void playAnimationAndSetBehaviorTimer(string animationName, float behaviourTimer)
    {
        animator.Play(animationName);
        this.behaviourTimer = behaviourTimer - behaviourTimer * Mathf.Clamp(agressiveMeter * 1 / 2, 0, 0.5f);
    }

    #region To Player Funcs

    void faceToPlayer()
    {
        if (playerTransform == null) return;

        Vector3 direction = playerTransform.position - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    float getDistanceToPlayer()
    {
        if (playerTransform == null) return 0;
        return Vector3.Distance(transform.position, playerTransform.position);
    }
    #endregion

    #region All Skills

    public void shootGrenade()
    {
        faceToPlayer();
        AudioManager.Instance.PlaySound("boss_shoot_grenade", gameObject);
        for (int i = 0; i < allGrenadeFirePoints.Length; i++)
        {
            Instantiate(shootGrenadeVFX, allGrenadeFirePoints[i].position, Quaternion.identity);
            for (int j = 0; j < 5; j++)
            {
                Rigidbody grenadeRB = Instantiate(grenadePrefab, allGrenadeFirePoints[i].position, Quaternion.identity).GetComponent<Rigidbody>();
                if (grenadeRB != null)
                {
                    Vector3 shootDir = allGrenadeFirePoints[i].forward;
                    grenadeRB.velocity = shootDir * grenadeSpeed * Random.Range(0.1f, 1.5f);
                    Vector3 torque = new Vector3(10, 10, 0);
                    grenadeRB.AddTorque(torque, ForceMode.Impulse);
                }
            }
        }
    }

    public void superBomb()
    {
        Instantiate(summonVFX, transform.position + new Vector3(0, 12, 0), Quaternion.identity);
        AudioManager.Instance.PlaySound("boss_summon_super_bomb", gameObject);
        Vector3 dropPos = playerTransform.position;
        dropPos.y = 300;
        Instantiate(superBombPrefab, dropPos, Quaternion.LookRotation(Vector3.down));
    }

    public void shootMissile()
    {
        faceToPlayer();
        AudioManager.Instance.PlaySound("missile_launch", gameObject);
        float randomZRotation = Random.Range(-40f, 40f);
        float currentX = missileFirePoint.eulerAngles.x;
        float currentY = missileFirePoint.eulerAngles.y;
        missileFirePoint.eulerAngles = new Vector3(currentX, currentY, randomZRotation);

        Vector3 shootDir = missileFirePoint.up;
        Rigidbody missileRB = Instantiate(missilePrefab, missileFirePoint.position, Quaternion.LookRotation(shootDir)).GetComponent<Rigidbody>();
        if (missileRB != null)
        {
            missileRB.velocity = shootDir * missileSpeed;
        }

        Missile missile = missileRB.GetComponent<Missile>();
        if (missile != null && playerTransform != null)
        {
            missile.setFollowTarget(playerTransform);
        }
    }

    public void Teleport()
    {
        createTeleportEffect();

        Vector3 teleportPos = GetTeleportPosition();

        AudioManager.Instance.createSFXgameObject("boss_teleport", transform.position);

        if (teleportPos != Vector3.zero)
        {
            transform.position = teleportPos;
        }

        faceToPlayer();

        AudioManager.Instance.PlaySound("boss_teleport", gameObject);
        createTeleportEffect();
    }

    private Vector3 GetTeleportPosition()
    {
        Vector3 teleportPos = Vector3.zero;

        for (int i = 0; i < 100; i++) // Lặp tối đa 100 lần để thử tìm vị trí hợp lệ
        {
            Vector3 randomDirection = Random.insideUnitSphere * teleportRadius;
            randomDirection.y = 0;

            Vector3 targetPosition = playerTransform.position + randomDirection;
            targetPosition.y = 0;

            if (!Physics.CheckSphere(targetPosition, 0.05f, avoidTeleportLayer))
            {
                teleportPos = targetPosition;
                break;
            }
        }

        return teleportPos;
    }

    void createTeleportEffect()
    {
        Instantiate(teleportVFX, transform.position + new Vector3(0, 3, 0), Quaternion.identity);
    }
    #endregion

    #region Damage Funcs
    public override void Damage(int damage)
    {
        base.Damage(damage);
        if (isDead) return;

        if (currentHealth <= maxHealth / 2 && currentHealth >= maxHealth / 4)
        {
            agressiveMeter = 0.5f;
        }
        else if (currentHealth < maxHealth / 4)
        {
            agressiveMeter = 1;
        }
        animator.speed = 1 + agressiveMeter;

        if (Random.value <= 0.1f) itemSpawner.Spawn();
    }

    public override void doSomethingThenDie()
    {
        base.doSomethingThenDie();
        animator.speed = 0.1f;
        animator.Play("boss_death");
        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }
        healthBar.gameObject.SetActive(false);
        StartCoroutine(TriggerDieExplosions());
        StartCoroutine(WaitThenGameOver());
    }
    #endregion

    #region All IEnumerators
    IEnumerator TriggerDieExplosions()
    {
        int exploCount = 15;
        float restTime = 0.25f;
        int newScale = 1;

        for (int i = 0; i < exploCount; i++)
        {
            Vector3 centerPos = transform.position;
            centerPos.y += 5;
            Vector3 randomPosition = centerPos + Random.insideUnitSphere * 3;

            if(i >= exploCount - 5)
            {
                restTime = 0.15f;
                newScale = 3;
                if(i == exploCount - 1)
                {
                    newScale = 20;
                }
            }
            Instantiate(deathExplosion, randomPosition, Quaternion.identity).transform.localScale = Vector3.one * newScale;
            yield return new WaitForSeconds(restTime);
        }
    }
    IEnumerator WaitThenGameOver()
    {
        PlayerPrefs.SetString("Game Result", "Win");
        yield return new WaitForSeconds(7f);
        SceneManager.LoadScene("Game Over");
    }

    IEnumerator WaitThenStartBoss(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        isBossStarted = true;
    }
    #endregion
}
