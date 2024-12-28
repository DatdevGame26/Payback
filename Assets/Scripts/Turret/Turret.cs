using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] protected float attackRange;
    [SerializeField] protected float fireRate;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected LayerMask obstacleLayer;
    [SerializeField] protected bool ignoreObstacle = false;
    [SerializeField] protected bool canShootFlyingEnemy = true;

    protected Transform currentTarget;
    protected float timer;

    protected virtual void Awake()
    {
        timer = 1;
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        timer -= Time.deltaTime;
        if (currentTarget == null)
        {
            findEnemyInAttackRange();
            return;
        }
        else
        {
            LiveEntity liveEntity = currentTarget.GetComponent<LiveEntity>();
            if (liveEntity != null && liveEntity.IsDead())
            {
                currentTarget = null;
                return;
            }
        }

        if (canAttackEnemy())
        {
            Attack();
            timer = fireRate;
        }

    }

    protected virtual bool canAttackEnemy()
    {
        if (currentTarget == null) return false;
        return Vector3.Distance(transform.position, currentTarget.position) <= attackRange
            && timer < 0;
    }

    protected virtual void Attack()
    {
    }

    protected virtual void findEnemyInAttackRange()
    {
        Collider[] allColliders = Physics.OverlapSphere(transform.position, attackRange);
        List<Transform> allEnemyTransforms = new List<Transform>();
        foreach (Collider collider in allColliders)
        {
            if (collider.tag == "Enemy")
            {
                allEnemyTransforms.Add(collider.transform);
            }
        }
        if (allEnemyTransforms.Count == 0) return;
        else
        {
            currentTarget = getClosetEnemyInAttackRange(allEnemyTransforms);
        }
    }

    protected Transform getClosetEnemyInAttackRange(List<Transform> allEnemyTransforms)
    {
        Transform closestEnemy = null;
        float closestDistance = attackRange;
        foreach (Transform enemy in allEnemyTransforms)
        {
            if (!canShootFlyingEnemy && enemy.position.y > 5) continue;

            float distance = Vector3.Distance(transform.position, enemy.position);
            if (!ignoreObstacle)
            {
                Vector3 startRaycastPos = transform.position;
                startRaycastPos.y = 1.35f;
                Vector3 directionToEnemy = (enemy.position - startRaycastPos).normalized;
                if (Physics.Raycast(startRaycastPos, directionToEnemy, distance, obstacleLayer))
                {
                    continue;
                }
            }

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }
        return closestEnemy;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
