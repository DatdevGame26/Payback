using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//      Trụ phóng tên lửa và Boss dùng Tên lửa
public class Missile : Bullet
{
    [Header("Missile")]
    [SerializeField] float delayFollow;  //  Khoảng thời gian từ lúc tạo sau bao lâu thì sẽ đuổi theo mục tiêu
    [SerializeField] float turnSpeed = 3f; // Tốc độ quay
    [SerializeField] float stopFollowAfterTime; //  Khoảng thời gian sau bao lâu thì ngừng đuổi theo mục tiêu  

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

            //  Khoảng cách tới mục tiêu
            float distanceToTarget = Vector3.Distance(lastTargetPos, transform.position);
            //  Hướng đi của tên lửa
            followDirection = (lastTargetPos - transform.position).normalized;

            //  Càng gần mục tiêu thì tăng tốc độ xoay
            if (distanceToTarget > 1f && distanceToTarget <= 50)
            {
                turnSpeed = 20f;
            }
            //  Kích nổ khi đạt khoảng cách nhất định
            else if (distanceToTarget < 2f)
            {
                createHitEffectAndDestroy();
            }
            //  Tính toán quay theo hướng cần đi 
            Quaternion targetRotation = Quaternion.LookRotation(followDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            rb.velocity = transform.forward * rb.velocity.magnitude;
        }
    }

    //  Đặt mục tiêu đuổi theo
    public void setFollowTarget(Transform target)
    {
        if (target == null) return;
        followTarget = target;
        lastTargetPos = target.position;
    }

    //  Đợi và đuổi theo mục tiêu
    IEnumerator waitThenFollowTarget()
    {
        yield return new WaitForSeconds(delayFollow);
        startFollow = true;
        if(stopFollowAfterTime != 0)
        {
            StartCoroutine(waitThenStopFollowTarget());
        } 
    }

    //  Đợi và ngừng đuổi theo mục tiêu
    IEnumerator waitThenStopFollowTarget()
    {
        yield return new WaitForSeconds(stopFollowAfterTime);
        startFollow = false;
    }
}
