using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] HostileTag hostileTag;
    [SerializeField] int bulletDamage;
    [SerializeField] bool isEnergy;
    [SerializeField] GameObject hitEffect;
    void Start()
    {
        Destroy(gameObject, 4);
    }

    private void OnTriggerEnter(Collider other)
    {
 
        if (other.tag == "Obstacle")
        {
            createHitEffectAndDestroy();
        }
        else if (other.tag == hostileTag.ToString())
        {
            IDamageable hostileEntity = other.GetComponent<IDamageable>();
            if (hostileEntity != null)
            {
                hostileEntity.Damage(bulletDamage);
                createHitEffectAndDestroy();
            }
        }
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
