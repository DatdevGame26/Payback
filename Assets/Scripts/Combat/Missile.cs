using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Bullet
{
    [Header("Missile")]
    [SerializeField] float delayFollow;
    [SerializeField] float turnSpeed = 3f; // Tốc độ quay
    [SerializeField] float stopFollowAfterTime;

    Transform followTarget;
    Rigidbody rb;

    Vector3 lastTargetPos;
    Vector3 followDirection;
    bool startFollow;
    protected override void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(waitThenFollowTarget());
        Destroy(gameObject, 15);
    }

    private void Update()
    {
        if (startFollow)
        {
            // Tính hướng về mục tiêu
            if (followTarget != null) { lastTargetPos = followTarget.position; }

            float distanceToTarget = Vector3.Distance(lastTargetPos, transform.position);
            followDirection = (lastTargetPos - transform.position).normalized;

            if (distanceToTarget > 1f && distanceToTarget <= 50)
            {
                turnSpeed = 20f;
            }
            else if (distanceToTarget < 2f)
            {
                createHitEffectAndDestroy();
            }

            Quaternion targetRotation = Quaternion.LookRotation(followDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            rb.velocity = transform.forward * rb.velocity.magnitude;
        }
    }

    public void setFollowTarget(Transform target)
    {
        if (target == null) return;
        followTarget = target;
        lastTargetPos = target.position;
    }

    IEnumerator waitThenFollowTarget()
    {
        yield return new WaitForSeconds(delayFollow);
        startFollow = true;
        if(stopFollowAfterTime != 0)
        {
            StartCoroutine(waitThenStopFollowTarget());
        } 
    }

    IEnumerator waitThenStopFollowTarget()
    {
        yield return new WaitForSeconds(stopFollowAfterTime);
        startFollow = false;
    }
}
