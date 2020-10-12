using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class UpgradeStats : MonoBehaviour
{
    public static int damage = 1;
    public static int health = 1;
    public static int coinsMultiplier = 1;

    public static int damageUpgradeCost = 10;
    public static int healthUpgradeCost = 10;
    public static int coinsMultiplierUpgradeCost = 10;

    private static float upgradeCostIncrease = 1.5f;

    public static UnityEvent damageUpgrade = new UnityEvent();
    public static UnityEvent healthUpgrade = new UnityEvent();
    public static UnityEvent coinsUpgrade = new UnityEvent();

    public static GameObject upgradeParticle = null;

    public static void UpgradeDamage()
    {
        upgradeParticle = Resources.Load<GameObject>("Particles/Upgrade Particle");
        Instantiate(upgradeParticle, UpgradeMenu.damageUpgradeBtn.transform.position, Quaternion.identity, UpgradeMenu.damageUpgradeBtn.transform);
        GameManager.CollectCoins(-damageUpgradeCost);
        damage++;
        damageUpgradeCost = Mathf.CeilToInt(damageUpgradeCost * upgradeCostIncrease);
        damageUpgrade.Invoke();
        AudioManager.PlayUpgradeSound();

        SaveSystem.SaveData();
    }

    public static void UpgradeHealth()
    {
        upgradeParticle = Resources.Load<GameObject>("Particles/Upgrade Particle");
        Instantiate(upgradeParticle, UpgradeMenu.healthUpgradeBtn.transform.position, Quaternion.identity, UpgradeMenu.healthUpgradeBtn.transform);
        GameManager.CollectCoins(-healthUpgradeCost);
        health++;
        healthUpgradeCost = Mathf.CeilToInt(healthUpgradeCost * upgradeCostIncrease);
        healthUpgrade.Invoke();
        AudioManager.PlayUpgradeSound();

        SaveSystem.SaveData();
    }

    public static void UpgradeCoinsMultiplier()
    {
        upgradeParticle = Resources.Load<GameObject>("Particles/Upgrade Particle");
        Instantiate(upgradeParticle, UpgradeMenu.coinsUpgradeBtn.transform.position, Quaternion.identity, UpgradeMenu.coinsUpgradeBtn.transform);
        GameManager.CollectCoins(-coinsMultiplierUpgradeCost);
        coinsMultiplier++;
        coinsMultiplierUpgradeCost = Mathf.CeilToInt(coinsMultiplierUpgradeCost * upgradeCostIncrease);
        coinsUpgrade.Invoke();
        AudioManager.PlayUpgradeSound();

        SaveSystem.SaveData();
    }

    public static void LoadStats(SaveData data)
    {
        damage = data.damageLevel;
        coinsMultiplier = data.coinsLevel;
        health = data.healthLevel;

        damageUpgradeCost = Mathf.CeilToInt(damageUpgradeCost * Mathf.Pow(upgradeCostIncrease, damage - 1));
        damageUpgrade.Invoke();
        healthUpgradeCost = Mathf.CeilToInt(healthUpgradeCost * Mathf.Pow(upgradeCostIncrease, health - 1));
        healthUpgrade.Invoke();
        coinsMultiplierUpgradeCost = Mathf.CeilToInt(coinsMultiplierUpgradeCost * Mathf.Pow(upgradeCostIncrease, coinsMultiplier - 1));
        coinsUpgrade.Invoke();
    }
}
