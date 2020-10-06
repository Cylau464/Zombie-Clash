using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField] private Text damageLevel = null;
    [SerializeField] private Text damageCost = null;
    [SerializeField] private Button damageUpgradeBtn = null;
    [SerializeField] private Text healthLevel = null;
    [SerializeField] private Text healthCost = null;
    [SerializeField] private Button healthUpgradeBtn = null;
    [SerializeField] private Text coinsMultiplierLevel = null;
    [SerializeField] private Text coinsMultiplierCost = null;
    [SerializeField] private Button coinsUpgradeBtn = null;

    private void Start()
    {
        DamageUpgrade();
        HealthUpgrade();
        CoinsUpgrade();

        UpgradeStats.damageUpgrade.AddListener(DamageUpgrade);
        UpgradeStats.healthUpgrade.AddListener(HealthUpgrade);
        UpgradeStats.coinsUpgrade.AddListener(CoinsUpgrade);
    }

    private void DamageUpgrade()
    {
        damageLevel.text = UpgradeStats.damage.ToString();
        damageCost.text = UpgradeStats.damageUpgradeCost.ToString();

        if (UpgradeStats.damageUpgradeCost > GameManager.Coins)
            damageUpgradeBtn.interactable = false;
        else
            damageUpgradeBtn.interactable = true;
    }

    private void HealthUpgrade()
    {
        healthLevel.text = UpgradeStats.health.ToString();
        healthCost.text = UpgradeStats.healthUpgradeCost.ToString();

        if (UpgradeStats.healthUpgradeCost > GameManager.Coins)
            healthUpgradeBtn.interactable = false;
        else
            healthUpgradeBtn.interactable = true;
    }

    private void CoinsUpgrade()
    {
        coinsMultiplierLevel.text = UpgradeStats.coinsMultiplier.ToString();
        coinsMultiplierCost.text = UpgradeStats.coinsMultiplierUpgradeCost.ToString();

        if (UpgradeStats.coinsMultiplierUpgradeCost > GameManager.Coins)
            coinsUpgradeBtn.interactable = false;
        else
            coinsUpgradeBtn.interactable = true;
    }
}
