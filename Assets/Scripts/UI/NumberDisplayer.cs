using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumberDisplayer : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] TextMeshProUGUI ammoText;
    [SerializeField] TextMeshProUGUI grenadeText;
    [SerializeField] TextMeshProUGUI fuelText;

    PlayerWeapon playerWeapon;
    TurretPlacer playerTurretPlacer;
    void Start()
    {
        playerWeapon = player.GetComponent<PlayerWeapon>();   
        playerTurretPlacer = player.GetComponent<TurretPlacer>();   
    }

    void Update()
    {
        if (player)
        {
            ammoText.text = playerWeapon.getAmmoSlashTotal();
            fuelText.text = playerTurretPlacer.getTotalFuel();
            grenadeText.text = playerWeapon.getGrenadeLeft();
        }
    }
}
