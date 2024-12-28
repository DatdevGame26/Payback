using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvent : MonoBehaviour
{
    NormalEnemy enemy;
    void Start()
    {
        enemy = GetComponentInParent<NormalEnemy>();
    }

    void finishAttackAnim()
    {
        enemy.finishAttackAnim();
    }

    void destroyParent()
    {
        enemy.Die();
    }

    void dealMeleeDamage()
    {
        MeleeEnemy meleeEnemy = enemy.GetComponent<MeleeEnemy>();
        if (meleeEnemy != null)
        {
            meleeEnemy.dealMeleeDamage();
        }
    }

    void blowUp()
    {
        BomberEnemy bomberEnemy = enemy.GetComponent<BomberEnemy>();
        if (bomberEnemy != null)
        {
            bomberEnemy.blowUp();
        }
    }

    public void playSound(string soundName)
    {
        AudioManager.Instance.PlaySound(soundName, enemy.gameObject);
    }

}
