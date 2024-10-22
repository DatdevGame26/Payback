using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealhBar : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Image healthImage;
    void Start()
    {
        
    }

    void Update()
    {
        if (player == null) return;
        healthImage.fillAmount = (float) player.getCurrentHealth() / player.getMaxHealth();
    }
}
