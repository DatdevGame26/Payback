using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretMissile : Turret
{
    [Header("Turret Missile")]
    [SerializeField] GameObject missilePrefab;
    [SerializeField] float missileSpeed;
    [SerializeField] Transform missileAnimationGO;
    [SerializeField] Vector2 startEndPosY;

    Vector3 missileAnimationPos;
    protected override void Awake()
    {
        base.Awake();
        missileAnimationPos = missileAnimationGO.localPosition;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        missileAnimationPos.y = Mathf.Lerp(startEndPosY.x, startEndPosY.y, 2 - (2 * timer / fireRate));
        missileAnimationGO.localPosition = missileAnimationPos;

    }

    protected override void Attack()
    {
        base.Attack();
        Missile newMissile = Instantiate(missilePrefab, firePoint.position, Quaternion.LookRotation(Vector3.up)).GetComponent<Missile>();
        if(newMissile != null)
        {
            newMissile.setFollowTarget(currentTarget);
        }
        Rigidbody missileRb = newMissile.GetComponent<Rigidbody>();
        if(missileRb != null)
        {
            missileRb.velocity = Vector3.up * missileSpeed;
        }
        AudioManager.Instance.PlaySound("missile_launch", gameObject);
    }

}
