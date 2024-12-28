using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : NormalEnemy
{
    [SerializeField] int meleeDamage;
    protected Player player;
    protected override void Awake()
    {
        base.Awake();
        player = playerTransform.GetComponent<Player>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
    protected override void Attack()
    {
        base.Attack();
    }

    public void dealMeleeDamage()
    {
        if (isPlayerInAttackRange())
        {
            player.Damage(meleeDamage);
        }
    }
}
