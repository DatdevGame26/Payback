using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] HostileTag hostileTag;
    [SerializeField] int explosionDamage;
    [SerializeField] string soundName;  //  Âm thanh lúc nổ
    [SerializeField] float exploMaxSoundRange = 50;  // Người chơi đi quá khoảng cách này thì sẽ không nghe thấy tiếng nổ nữa
    private void Start()
    {
        StartCoroutine(waitThenTurnOffCollider());
        //  Chơi nhạc
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

    //  Tắt Collider đi để tránh gây sát thương nhiều lần
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
