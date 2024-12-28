using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretWave : Turret
{
    [SerializeField] GameObject wave;
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
        base.Attack();
        Instantiate(wave, firePoint.position, Quaternion.identity);
        AudioManager.Instance.PlaySound("turret_wave", gameObject);
    }

}
