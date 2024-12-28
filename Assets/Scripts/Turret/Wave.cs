using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : Bullet
{
    protected override void additionalEffect()
    {
        base.additionalEffect();
        Enemy enemy = hitCollider.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.setNegativeEffect();
        }
    }
}
