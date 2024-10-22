using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberEnemy : Enemy
{
    [SerializeField] GameObject Explosion;
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
    }
    protected override void Attack()
    {
        Instantiate(Explosion, transform.position, Quaternion.identity);
        Die();
    }

    public override void doSomethingThenDie()
    {
        Attack();
    }
}
