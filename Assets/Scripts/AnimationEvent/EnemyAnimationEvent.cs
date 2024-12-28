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


    //  Báo hiệu đã thực hiện animation tấn công xong để enemy nghỉ
    void finishAttackAnim()
    {
        enemy.finishAttackAnim();
    }


    //  Đặt ở đoạn cuối animation chết của enemy để tự xoá enemy khỏi game
    void destroyParent()
    {
        enemy.Die();
    }


    //  Gây sát thương tầm gần (chỉ với robot đấm)
    void dealMeleeDamage()
    {
        MeleeEnemy meleeEnemy = enemy.GetComponent<MeleeEnemy>();
        if (meleeEnemy != null)
        {
            meleeEnemy.dealMeleeDamage();
        }
    }


    //  Tự nổ và xoá enemy khỏi game (chỉ với robot bom)
    void blowUp()
    {
        BomberEnemy bomberEnemy = enemy.GetComponent<BomberEnemy>();
        if (bomberEnemy != null)
        {
            bomberEnemy.blowUp();
        }
    }


    //  Phát ra âm thanh chỉ định bằng soundName
    public void playSound(string soundName)
    {
        AudioManager.Instance.PlaySound(soundName, enemy.gameObject);
    }

}
