using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : NormalEnemy
{
    [Header("Enemy: Shooter")]
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletSpeed;
    [SerializeField] Transform firePoint;
    [SerializeField] LayerMask obstacleLayer;

    float baseAttackRange;
    protected override void Awake()
    {
        base.Awake();
        baseAttackRange = attackRange;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        firePoint.LookAt(playerTransform.position);
        detectObstacle();

    }

    private void detectObstacle()
    {
        RaycastHit hit;
        bool hitAnything = Physics.Raycast(firePoint.position, firePoint.forward, out hit, baseAttackRange);
        if (hitAnything)
        {
            if (((1 << hit.collider.gameObject.layer) & obstacleLayer) != 0)
            {
                attackRange = 1f;
            }
            else
            {
                attackRange = baseAttackRange;
            }
        }
        else
        {
            attackRange = baseAttackRange;
        }
    }

    protected override void Attack()
    {
        base.Attack();
        animator.SetTrigger("Attack");
        AudioManager.Instance.PlaySound("enemy_shooter", gameObject);
        Vector3 shootDir = firePoint.transform.forward;

        float spreadAngle = (float)((1 - 0.8f) * 10);
        shootDir.x += Random.Range(-spreadAngle, spreadAngle) * 0.01f;
        shootDir.y += Random.Range(-spreadAngle, spreadAngle) * 0.01f;

        Rigidbody bulletRB = Instantiate(bullet, firePoint.position, Quaternion.LookRotation(shootDir)).GetComponent<Rigidbody>();
        if(bulletRB != null )
        {
            bulletRB.velocity = shootDir * bulletSpeed;
        }
    }
}
