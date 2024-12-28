using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//      UI hiển thị máu, đạn, nhiên liệu của người chơi
public class NumberDisplayer : MonoBehaviour
{
    [SerializeField] GameObject playerGO;
    [SerializeField] TextMeshProUGUI ammoText;
    [SerializeField] TextMeshProUGUI grenadeText;
    [SerializeField] TextMeshProUGUI fuelText;
    [SerializeField] TextMeshProUGUI healthText;

    PlayerWeapon playerWeapon;
    Player player;
    void Start()
    {
        playerWeapon = playerGO.GetComponent<PlayerWeapon>();
        player = playerGO.GetComponent<Player>();   
    }

    void Update()
    {
        if (playerGO)
        {
            ammoText.text = playerWeapon.getAmmoSlashTotal();
            fuelText.text = player.getCurrentFuel().ToString();
            grenadeText.text = playerWeapon.getGrenadeLeft();
            healthText.text = $"{player.getCurrentHealth()} / {player.getMaxHealth()}";
        }
    }
}
