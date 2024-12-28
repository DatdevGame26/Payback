using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/* Đây là lớp kẻ thù (Robot), kẻ thù là thực thể sống, chúng có thể:
 *    Tự động tìm kiếm người chơi và đi đến người chơi (dùng AI Nav Mesh truy tìm)
 *    Có tầm tấn công và nếu người chơi nằm trong tầm này và không vướng chướng ngại vật, sẽ tấn công người chơi
 *    Khi tấn công sẽ có khoảng dừng nhỏ trước khi tấn công người chơi lại
 *    Chịu hiệu ứng xấu: giảm tốc chạy và nhận thêm sát thương
 * Hiện tại Enemy là lớp cha, sau này có nhiều loại kẻ thù sẽ có cách tấn công khác nhau nên sẽ kế thừa từ lớp này
 */

public class NormalEnemy : Enemy
{
    [Header("Enemy Combat Info")]
    [SerializeField] protected float attackRange;
    [SerializeField] float restTime;

    protected NavMeshAgent agent;
    protected bool isAttacking;
    protected float baseSpeed;

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
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

        if (isStopAction()) return;

        agent.SetDestination(playerTransform.position);
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

        AudioManager.Instance.createSFXgameObject("robot_death", transform.position, 40);

        itemSpawner.Spawn();
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
        if (playerTransform == null) return false;
        return Vector3.Distance(playerTransform.position, transform.position) <= attackRange;
    }

    protected void FacePlayerY()
    {
        if (playerTransform == null) return;

        Vector3 playerDir = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z) - transform.position;
        playerDir.Normalize();
        Quaternion lookRotation = Quaternion.LookRotation(playerDir);
        float targetYRotation = lookRotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, targetYRotation, transform.rotation.eulerAngles.z);
    }
}
