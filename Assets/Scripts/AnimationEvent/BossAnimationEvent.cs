using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationEvent : MonoBehaviour
{

    [SerializeField] Boss boss; // Tham chiếu đến Boss

    void Teleport()
    {
        //  Gọi hàm để boss dịch chuyển tức thời
        boss.Teleport();
    }

    void shootMissile()
    {
        //  Gọi hàm để boss bắn một tên lửa
        boss.shootMissile();
    }

    void superBomb()
    {
        //  Gọi hàm để boss triệu hồi siêu bom
        boss.superBomb();
    }

    void throwGrenade()
    {
        //  Gọi hàm để boss bắn lựu đạn
        boss.shootGrenade();    
    }
}
