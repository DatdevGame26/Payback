using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationEvent : MonoBehaviour
{
    [SerializeField] PlayerWeapon weapon;
    void FinishReload()
    {
        weapon.finishReload();
    }
}
