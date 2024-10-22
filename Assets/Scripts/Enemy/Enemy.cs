using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : LiveEntity
{
    [SerializeField] GameObject negativeEffectObject;
    [SerializeField] GameObject deathExplosion;
    [Header("Enemy Animator")]
    [SerializeField] protected float animationSpeed = 1;
    [SerializeField] protected Animator animator;
    [Header("Enemy Combat Info")]
    [SerializeField] protected float attackRange;
    [SerializeField] float restTime;

    protected NavMeshAgent agent;
    protected Transform playerT;
    protected bool isAttacking;
    protected float baseSpeed;
    float negativeTimer;
    bool hasNegativeEffect;
    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        playerT = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void Start()
    {
        base.Start();
        baseSpeed = agent.speed;
        if(animator != null)
        {
            animator.speed = animationSpeed;
        }
    }

    protected override void Update()
    {
        base.Update();
        if (playerT == null || isDead) return;

        agent.SetDestination(playerT.position);
        if (isAttacking) agent.speed = 0;
        else
        {
            if(hasNegativeEffect) agent.speed = baseSpeed / 2;
            else agent.speed = baseSpeed;
        }

        if (isPlayerInAttackRange())
        {
            FacePlayerY();
            if (!isAttacking)
            {
                Attack();
            }
        }

        if (animator != null) animator.SetBool("Run", isRunning());

        negativeEffectObject.SetActive(hasNegativeEffect);
        hasNegativeEffect = negativeTimer > 0;
        negativeTimer-=Time.deltaTime;
    }

    protected bool isRunning() { return agent.velocity.magnitude > 0.01f; }

    protected virtual void Attack()
    {
        isAttacking = true;
        animator.SetBool("Attack", true);
        StartCoroutine(WaitThenFinishAttack());
    }

    public override void doSomethingThenDie()
    {
        base.doSomethingThenDie();
        if(deathExplosion != null)
        {
            Instantiate(deathExplosion, transform.position, Quaternion.identity);
        }

        GetComponent<Collider>().enabled = false;

        animator.SetBool("Attack", false);
        animator.SetBool("Run", false);
        animator.SetBool("Dead", true);

        agent.speed = 0;
    }

    IEnumerator WaitThenFinishAttack()
    {
        yield return new WaitForSeconds(restTime);
        agent.SetDestination(transform.position);
        animator.SetBool("Attack", false);
        isAttacking = false;

    }

    public void finishAttackAnim() { animator.SetBool("Attack", false); }

    protected bool isPlayerInAttackRange()
    {
        if (playerT == null) return false;
        return Vector3.Distance(playerT.position, transform.position) <= attackRange;
    }

    protected void FacePlayerY()
    {
        if (playerT == null) return;

        Vector3 playerDir = new Vector3(playerT.position.x, transform.position.y, playerT.position.z) - transform.position;
        playerDir.Normalize();
        Quaternion lookRotation = Quaternion.LookRotation(playerDir);
        float targetYRotation = lookRotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, targetYRotation, transform.rotation.eulerAngles.z);
    }

    public override void Damage(int damage)
    {
        if (hasNegativeEffect)
        {
            int additionalDamage = Mathf.CeilToInt(damage * 0.5f);
            int totalDamage = damage + additionalDamage;
            base.Damage(totalDamage);
        }
        else
        {
            base.Damage(damage);
        }
    }

    public void setNegativeEffect()
    {
        negativeTimer = 3;
    }
}
