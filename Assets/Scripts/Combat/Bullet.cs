using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] HostileTag hostileTag;
    [SerializeField] protected int bulletDamage;
    [SerializeField] bool isEnergy;
    [SerializeField] GameObject hitEffect;
    [SerializeField] float lifeSpan = 4;

    protected Collider hitCollider;

    protected virtual void Start()
    {
        Destroy(gameObject, lifeSpan);
    }

    private void OnTriggerEnter(Collider other)
    {
        hitCollider = other;
        if (hitCollider.tag == "Obstacle")
        {
            createHitEffectAndDestroy();
        }
        else if (hitCollider.tag == hostileTag.ToString())
        {
            IDamageable hostileEntity = hitCollider.GetComponent<IDamageable>();
            if (hostileEntity != null)
            {
                hostileEntity.Damage(bulletDamage);
                additionalEffect();
                createHitEffectAndDestroy();
            }
        }
    }

    protected virtual void additionalEffect()
    {
        
    }


    protected void createHitEffectAndDestroy()
    {
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }
        if (!isEnergy) Destroy(gameObject);
    }
}



public enum HostileTag
{
    Enemy, Player
}
