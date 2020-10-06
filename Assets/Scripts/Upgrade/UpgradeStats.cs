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

    public static void UpgradeDamage()
    {
        damage++;
        damageUpgradeCost = Mathf.CeilToInt(damageUpgradeCost * upgradeCostIncrease);
        damageUpgrade.Invoke();
    }

    public static void UpgradeHealth()
    {
        health++;
        healthUpgradeCost = Mathf.CeilToInt(healthUpgradeCost * upgradeCostIncrease);
        healthUpgrade.Invoke();
    }

    public static void UpgradeCoinsMultiplier()
    {
        coinsMultiplier++;
        coinsMultiplierUpgradeCost = Mathf.CeilToInt(coinsMultiplierUpgradeCost * upgradeCostIncrease);
        coinsUpgrade.Invoke();
    }
}
