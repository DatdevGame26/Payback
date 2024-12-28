using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//  UI nâng cấp súng người chơi
public class UpgradeGunPanel : MonoBehaviour
{
    [SerializeField] Image[] allStackImages;
    [SerializeField] GameObject priceGO;
    [SerializeField] TextMeshProUGUI upgradeTitle;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] Button plusButton;
    [SerializeField] Color upgradeColor;
    Color startColor;
    int currentLevel;

    bool isMaxLevel;
    private void Start()
    {
        startColor = new Color(upgradeColor.r, upgradeColor.g, upgradeColor.b, 0.027f);
        currentLevel = -1;
        foreach (var stackImg in allStackImages)
        {
            stackImg.color = startColor;
        }
    }

    public void Init(string title, int startPrice)
    {
        upgradeTitle.text = title;
        priceText.text = startPrice.ToString();
    }

    public void updatePrice(int newPrice)
    {
        priceText.text = newPrice.ToString(); 
    }

    public void plusLevel()
    {
        if (isMaxLevel) return;

        if (currentLevel <= allStackImages.Length - 1)
        {
            currentLevel++;
            allStackImages[currentLevel].color = upgradeColor;
            if (currentLevel == allStackImages.Length - 1)
            {
                plusButton.interactable = false;
                priceGO.SetActive(false);
                TextMeshProUGUI buttonText = plusButton.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.text = "Max";
                    buttonText.fontSize = 14;
                }
                isMaxLevel = true;
            }
        }
    }



}
