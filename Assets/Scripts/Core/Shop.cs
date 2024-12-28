using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Shop : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] PauseManager pauseManager;
    [Header("Shop")]
    [SerializeField] GameObject shopUI;
    [SerializeField] GameObject[] allTempTurretPrefabs;
    [SerializeField] GameObject notEnoughFuelWarning;

    [Header("Stats")]
    [SerializeField] Stat[] allStats;
    TurretPlacer turretPlacer;
    PlayerWeapon playerWeapon;

    bool isShopOpened;
    bool isWarningDisplayed;

    private void Awake()
    {
        for (int i = 0; i < allStats.Length; i++)
        {
            allStats[i] = new Stat(allStats[i].statName, allStats[i].upgradeGunPanel, allStats[i].allLevelPrices);
        }
    }

    void Start()
    {
        notEnoughFuelWarning.SetActive(false);
        shopUI.SetActive(false);
        isWarningDisplayed = false;
        if (player != null)
        {
            turretPlacer = player.GetComponent<TurretPlacer>();
            playerWeapon = player.GetComponent<PlayerWeapon>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) toggleShop();
    }

    public void buyTurret(int turretIndex)
    {
        TempTurret tempTurret = allTempTurretPrefabs[turretIndex].GetComponent<TempTurret>();
        if (tempTurret != null)
        {
            if (tempTurret.getFuelNeed() <= player.getCurrentFuel())
            {
                turretPlacer.spawnTempTurret(allTempTurretPrefabs[turretIndex]);
                toggleShop();
            }
            else displayWarning();
        }
    }

    void toggleShop()
    {
        if (pauseManager.isPaused) return;

        isShopOpened = !isShopOpened;
        player.stopController(isShopOpened);
        shopUI.SetActive(isShopOpened);

        notEnoughFuelWarning.SetActive(false);
        isWarningDisplayed = false;

        if (isShopOpened) Cursor.lockState = CursorLockMode.None;
        else Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = isShopOpened ? 0 : 1;
    }

    void displayWarning()
    {
        if (isWarningDisplayed) return;
        StartCoroutine(WaitThenDisableFuelWarning(3));
    }

    IEnumerator WaitThenDisableFuelWarning(float seconds)
    {
        notEnoughFuelWarning.SetActive(true);
        yield return new WaitForSeconds(seconds);
        notEnoughFuelWarning.SetActive(false);
    }

    public void UpgradeStat(int statIndex)
    {
        Stat chosenStat = allStats[statIndex];

        if(chosenStat.getUpgradePrice() > player.getCurrentFuel())
        {
            displayWarning();
            return;
        }

        player.minusFuel(chosenStat.getUpgradePrice());


        switch (statIndex)
        {
            case 0:
                playerWeapon.increaseFireRate();
                break;
            case 1:
                playerWeapon.increaseProjectileSpeed();
                break;
            case 2:
                playerWeapon.increaseAmmoCapacity();
                break;
            case 3:
                playerWeapon.increaseBulletDamage();
                break;
            default:
                playerWeapon.increaseFireRate();
                break;

        }

        chosenStat.Upgrade();
    }

    public bool IsShopOpened()
    {
        return isShopOpened;
    }

}



