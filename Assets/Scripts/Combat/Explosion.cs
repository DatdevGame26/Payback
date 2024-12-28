using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] HostileTag hostileTag;
    [SerializeField] int explosionDamage;
    [SerializeField] string soundName;
    [SerializeField] float exploMaxSoundRange = 50;
    private void Start()
    {
        StartCoroutine(waitThenTurnOffCollider());
        AudioManager.Instance.createSFXgameObject(soundName, transform.position, exploMaxSoundRange);
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
        Collider col = GetComponent<Collider>();
        yield return new WaitForSeconds(0.1f);
        if(col != null)
        {
            col.enabled = false;
        }
    }
}
