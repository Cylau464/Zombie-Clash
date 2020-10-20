using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class UpgradeStats : MonoBehaviour
{
    public static int damageLevel = 1;
    public static int healthLevel = 1;
    public static int coinsLevel = 1;
    public static int damageMultiplier = 0;
    public static int healthMultiplier = 0;
    public static int coinsMultiplier = 0;
    public static float healthScaleIncreaser = .01f;

    public static int damageUpgradeCost = 150;
    public static int healthUpgradeCost = 150;
    public static int coinsMultiplierUpgradeCost = 150;

    private static int upgradeCostIncrease = 150;

    public static UnityEvent damageUpgrade = new UnityEvent();
    public static UnityEvent healthUpgrade = new UnityEvent();
    public static UnityEvent coinsUpgrade = new UnityEvent();

    public static GameObject upgradeParticle = null;

    public static void UpgradeDamage()
    {
        upgradeParticle = Resources.Load<GameObject>("Particles/Upgrade Particle");
        Instantiate(upgradeParticle, UpgradeMenu.damageUpgradeBtn.transform.position, Quaternion.identity, UpgradeMenu.damageUpgradeBtn.transform);
        GameManager.CollectCoins(-damageUpgradeCost);
        damageLevel++;
        damageMultiplier++;
        damageUpgradeCost += Mathf.CeilToInt(upgradeCostIncrease * (Mathf.FloorToInt(damageLevel / 10f) + 1));
        damageUpgrade.Invoke();
        AudioManager.PlayUpgradeSound();

        SaveSystem.SaveData();
    }

    public static void UpgradeHealth()
    {
        upgradeParticle = Resources.Load<GameObject>("Particles/Upgrade Particle");
        Instantiate(upgradeParticle, UpgradeMenu.healthUpgradeBtn.transform.position, Quaternion.identity, UpgradeMenu.healthUpgradeBtn.transform);
        GameManager.CollectCoins(-healthUpgradeCost);
        healthLevel++;
        healthMultiplier++;
        healthUpgradeCost += Mathf.CeilToInt(upgradeCostIncrease * (Mathf.FloorToInt(healthLevel / 10f) + 1));
        healthUpgrade.Invoke();
        AudioManager.PlayUpgradeSound();

        SaveSystem.SaveData();
    }

    public static void UpgradeCoinsMultiplier()
    {
        upgradeParticle = Resources.Load<GameObject>("Particles/Upgrade Particle");
        Instantiate(upgradeParticle, UpgradeMenu.coinsUpgradeBtn.transform.position, Quaternion.identity, UpgradeMenu.coinsUpgradeBtn.transform);
        GameManager.CollectCoins(-coinsMultiplierUpgradeCost);
        coinsLevel++;
        coinsMultiplier++;
        coinsMultiplierUpgradeCost += Mathf.CeilToInt(upgradeCostIncrease * (Mathf.FloorToInt(coinsLevel / 10f) + 1));
        coinsUpgrade.Invoke();
        AudioManager.PlayUpgradeSound();

        SaveSystem.SaveData();
    }

    public static void LoadStats(SaveData data)
    {
        damageLevel = data.damageLevel;
        coinsLevel = data.coinsLevel;
        healthLevel = data.healthLevel;
        damageMultiplier = data.damageMultiplier;
        coinsMultiplier = data.coinsMultiplier;
        healthMultiplier = data.healthMultiplier;

        damageUpgradeCost = data.damageUpgradeCost;
        damageUpgrade.Invoke();
        healthUpgradeCost = data.healthUpgradeCost;
        healthUpgrade.Invoke();
        coinsMultiplierUpgradeCost = data.coinsUpgradeCost;
        coinsUpgrade.Invoke();
    }
}
