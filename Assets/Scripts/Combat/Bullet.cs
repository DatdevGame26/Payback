using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Nhãn kẻ thù, tức chỉ gây sát thương đối với GameObject có Tag hostileTag
public enum HostileTag
{
    Enemy, Player
}

//   Lớp đạn có thể dùng chung cho tất cả GameObject đạn của: Người chơi, Trụ súng, Kẻ thù, Boss
public class Bullet : MonoBehaviour
{
    [SerializeField] HostileTag hostileTag; //  Nhãn kẻ thù
    [SerializeField] protected int bulletDamage;    //  Sát thương đạn
    [SerializeField] bool isEnergy;     //  Xác định xem đạn này có thể đi xuyên vật cản không (chỉ sóng xung kích)
    [SerializeField] GameObject hitEffect;      //  Hiệu ứng khi đạn va chạm
    [SerializeField] float lifeSpan = 4;    //  Vòng đời của đạn

    protected Collider hitCollider;     //  Collider đạn va phải

    protected virtual void Start()
    {
        //  Tự xoá chính nó sau một khoảng lifeSpan
        Destroy(gameObject, lifeSpan);
    }

    //   Xử lý va chạm nếu trúng vật cản thì tự huỷ (trừ khi là isEnergy), nếu là thực thể có thể bị thương và trùng Tag hostileTag thì gây sát thương cho nó
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

    //     Hiệu ứng thêm vào (để virtual để có thể override ở lớp con)
    protected virtual void additionalEffect()
    {
        
    }

    //      Tạo hiệu ứng va chạm và tự huỷ
    protected void createHitEffectAndDestroy()
    {
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }
        if (!isEnergy) Destroy(gameObject);
    }
}
