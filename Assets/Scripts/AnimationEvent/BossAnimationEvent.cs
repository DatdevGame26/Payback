using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationEvent : MonoBehaviour
{

    [SerializeField] Boss boss;

    void Teleport()
    {
        boss.Teleport();
    }

    void shootMissile()
    {
        boss.shootMissile();
    }

    void superBomb()
    {
        boss.superBomb();
    }

    void throwGrenade()
    {
        boss.shootGrenade();    
    }
}
