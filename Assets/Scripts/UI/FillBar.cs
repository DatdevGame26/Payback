using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FillBar : MonoBehaviour
{
    [SerializeField] Image fillImage;
    [Header("Optional")]
    [SerializeField] TextMeshProUGUI displayValue;

    float currentValue;
    float maxValue;

    void Update()
    {
        if (fillImage == null || maxValue == 0) return;

        fillImage.fillAmount = currentValue / maxValue;

        if (displayValue != null)
        {
            displayValue.text = $"{currentValue} / {maxValue}";
        }
    }

    public void updateFillBar(float currentValue, float maxValue)
    {
        this.currentValue = currentValue;
        this.maxValue = maxValue;
    }

}
