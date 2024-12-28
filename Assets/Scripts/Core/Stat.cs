[System.Serializable]
public class Stat
{
    public string statName;
    public UpgradeGunPanel upgradeGunPanel;
    public int[] allLevelPrices;
    int currentLevel = 0;

    public Stat(string statName, UpgradeGunPanel upgradeGunPanel, int[] allLevelPrices)
    {
        this.statName = statName;
        this.upgradeGunPanel = upgradeGunPanel;
        this.allLevelPrices = allLevelPrices;

        upgradeGunPanel.Init(statName, allLevelPrices[0]);
    }

    public void Upgrade()
    {
        upgradeGunPanel.plusLevel();
        int nextLevel = currentLevel + 1;
        if (nextLevel >= allLevelPrices.Length) return;

        upgradeGunPanel.updatePrice(allLevelPrices[nextLevel]);
        currentLevel = nextLevel;
    }

    public int getUpgradePrice()
    {
        return allLevelPrices[currentLevel];
    }
}