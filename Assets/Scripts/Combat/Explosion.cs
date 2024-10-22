using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] HostileTag hostileTag;
    [SerializeField] int explosionDamage;
    private void Start()
    {
        StartCoroutine(waitThenTurnOffCollider());
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == hostileTag.ToString())
        {
            IDamageable damageable = other.GetComponent<IDamageable>(); 
            if(damageable != null)
            {
                damageable.Damage(explosionDamage);
            }
        }
    }

    IEnumerator waitThenTurnOffCollider()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<Collider>().enabled = false;
    }
}
