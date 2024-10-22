using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] PlayerWeapon playerWeapon;
    [SerializeField] Transform[] allLines;
    [SerializeField] GameObject sniperCrosshair;


    void Update()
    {
        if(playerWeapon == null) return;

        allLines[0].localPosition = new Vector3(0, getLinePosition(), 0);
        allLines[1].localPosition = new Vector3(getLinePosition(), 0, 0);
        allLines[2].localPosition = new Vector3(0, -getLinePosition(), 0);
        allLines[3].localPosition = new Vector3(-getLinePosition(), 0, 0);
    }


    float getLinePosition()
    {
        return (float)(-75 * playerWeapon.getAccuracy() + 100);
    }

    public void toggleSniperCrosshair(bool useScope)
    {
        sniperCrosshair.SetActive(useScope);
        for (int i = 0; i < allLines.Length; i++)
        {
            allLines[i].gameObject.SetActive(!useScope);
        }
    }
}