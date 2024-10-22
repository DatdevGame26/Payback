using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvent : MonoBehaviour
{
    Enemy enemy;
    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    void finishAttackAnim()
    {
        enemy.finishAttackAnim();
    }

    void destroyParent()
    {
        enemy.Die();
    }
}
