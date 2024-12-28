using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TurretShooter : Turret
{
    [Header("Turret Shooter")]
    [SerializeField] Transform gunTopTransform;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float rotateSpeed;
    [SerializeField] float bulletSpeed;
    [SerializeField] float accuracy;
    [SerializeField] protected GameObject muzzleFlash;

    Vector3 shootDirection;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (currentTarget != null)
        {
            caculateShootDirection();
            rotateGunTopToTarget();
            checkIfTargetIsBlocked();
        }
    }
    private void caculateShootDirection()
    {
        shootDirection = currentTarget.position - transform.position;
        shootDirection.Normalize();
        firePoint.transform.forward = shootDirection;
    }

    private void rotateGunTopToTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(shootDirection);
        targetRotation = Quaternion.Euler(-90f, targetRotation.eulerAngles.y, 0f);
        gunTopTransform.rotation = Quaternion.RotateTowards(gunTopTransform.rotation,
            targetRotation, rotateSpeed * Time.deltaTime);
    }

    protected override void Attack()
    {
        base.Attack();
        float spreadAngle = (float)((1 - accuracy) * 10);
        shootDirection.x += Random.Range(-spreadAngle, spreadAngle) * 0.01f;
        shootDirection.y += Random.Range(-spreadAngle, spreadAngle) * 0.01f;

        GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(shootDirection));
        newBullet.transform.forward = shootDirection;
        Rigidbody bulletRb = newBullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.velocity = shootDirection * bulletSpeed;
        }
        if (muzzleFlash != null)
        {
            Instantiate(muzzleFlash, firePoint.position, firePoint.rotation);
        }

        AudioManager.Instance.PlaySound("turret_shooter", gameObject);
    }

    protected override bool canAttackEnemy()
    {
        float rotateAngle = 180 - Vector3.Angle(gunTopTransform.up, shootDirection);
        if (rotateAngle >= 10) return false;
        return base.canAttackEnemy();
    }

    void checkIfTargetIsBlocked()
    {
        float distance = Vector3.Distance(transform.position, currentTarget.position);
        Vector3 startRaycastPos = transform.position;
        startRaycastPos.y = 1.35f;
        Vector3 directionToEnemy = (currentTarget.position - startRaycastPos).normalized;
        if (Physics.Raycast(startRaycastPos, directionToEnemy, distance, obstacleLayer))
        {
            currentTarget = null;
        }
    }
}
