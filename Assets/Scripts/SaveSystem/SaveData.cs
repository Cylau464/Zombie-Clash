using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int coins;
    public int keys;
    public int levelNumber;

    //Upgradable stats
    public int damageLevel;
    public int coinsLevel;
    public int healthLevel;
    public int damageUpgradeCost;
    public int coinsUpgradeCost;
    public int healthUpgradeCost;
    public int damageMultiplier = 1;
    public int healthMultiplier = 1;
    public float coinsMultiplier = 1;

    //Level properties
    public int defendersLeft = 0;
    public int bossHealthLeft = 0;
    public List<int> collectedKeys;

    public SaveData()
    {
        if (GameManager.current != null)
        {
            coins = GameManager.Coins;
            keys = GameManager.Keys;
        }

        levelNumber = LevelManager.LevelNumber;
        damageLevel = UpgradeStats.damageLevel;
        coinsLevel = UpgradeStats.coinsLevel;
        healthLevel = UpgradeStats.healthLevel;
        damageUpgradeCost = UpgradeStats.damageUpgradeCost;
        coinsUpgradeCost = UpgradeStats.coinsMultiplierUpgradeCost;
        healthUpgradeCost = UpgradeStats.healthUpgradeCost;
        damageMultiplier = UpgradeStats.damageMultiplier;
        healthMultiplier = UpgradeStats.healthMultiplier;
        coinsMultiplier = UpgradeStats.coinsMultiplier;

        defendersLeft = FightStage.defendersLeft;
        bossHealthLeft = FightStage.bossHealthLeft;

        collectedKeys = CollectableObjectContainer.keys;
    }
}
