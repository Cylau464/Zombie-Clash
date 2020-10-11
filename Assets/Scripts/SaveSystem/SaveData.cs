using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int coins;
    public int keys;
    public int levelIndex;

    //Upgradable stats
    public int damageLevel;
    public int coinsLevel;
    public int healthLevel;

    public SaveData()
    {
        if (GameManager.current != null)
        {
            coins = GameManager.Coins;
            keys = GameManager.Keys;
        }

        levelIndex = LevelManager.LevelIndex;
        damageLevel = UpgradeStats.damage;
        coinsLevel = UpgradeStats.coinsMultiplier;
        healthLevel = UpgradeStats.health;
    }
}
