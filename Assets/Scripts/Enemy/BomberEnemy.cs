using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberEnemy : NormalEnemy
{
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

    public override void doSomethingThenDie()
    {
        itemSpawner.Spawn();
        blowUp();
    }

    public void blowUp()
    {
        Instantiate(deathExplosion, transform.position, Quaternion.identity);
        Die();
    }
}
