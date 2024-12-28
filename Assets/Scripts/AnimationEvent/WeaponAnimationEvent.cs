using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationEvent : MonoBehaviour
{
    [SerializeField] PlayerWeapon weapon;

    //  Báo hiệu vũ khí người chơi đã nạp đạn xong để cho phép bắn tiếp (đặt ở cuối animation player_gun_reload)
    void FinishReload()
    {
        weapon.finishReload();
    }
}
