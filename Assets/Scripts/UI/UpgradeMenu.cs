using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _damageLevel = null;
    [SerializeField] private TextMeshProUGUI _damageCost = null;
    [Space]
    [SerializeField] private TextMeshProUGUI _coinsMultiplierLevel = null;
    [SerializeField] private TextMeshProUGUI _coinsMultiplierCost = null;
    [Space]
    [SerializeField] private TextMeshProUGUI _healthLevel = null;
    [SerializeField] private TextMeshProUGUI _healthCost = null;

    [Header("Buttons")]
    [SerializeField] private Button _damageUpgradeBtn = null;

    [SerializeField] private Button _coinsUpgradeBtn = null;

    [SerializeField] private Button _healthUpgradeBtn = null;

    [Space]
    [SerializeField] private CanvasGroup _canvasGroup = null;
    [SerializeField] private GraphicRaycaster _raycaster = null;

    private void Start()
    {
        DamageUpgrade();
        HealthUpgrade();
        CoinsUpgrade();

        GameManager.gameStart.AddListener(HideMenu);
        UpgradeStats.damageUpgrade.AddListener(DamageUpgrade);
        UpgradeStats.healthUpgrade.AddListener(HealthUpgrade);
        UpgradeStats.coinsUpgrade.AddListener(CoinsUpgrade);
    }

    private void DamageUpgrade()
    {
        _damageLevel.text = "Уровень " + UpgradeStats.damage.ToString();
        _damageCost.text = UpgradeStats.damageUpgradeCost.ToString();
        CheckNewCost();
    }

    private void HealthUpgrade()
    {
        _healthLevel.text = "Уровень " + UpgradeStats.health.ToString();
        _healthCost.text = UpgradeStats.healthUpgradeCost.ToString();
        CheckNewCost();
    }

    private void CoinsUpgrade()
    {
        _coinsMultiplierLevel.text = "Уровень " + UpgradeStats.coinsMultiplier.ToString();
        _coinsMultiplierCost.text = UpgradeStats.coinsMultiplierUpgradeCost.ToString();
        CheckNewCost();
    }

    private void CheckNewCost()
    {
        if (UpgradeStats.damageUpgradeCost > GameManager.Coins)
            _damageUpgradeBtn.interactable = false;
        else
            _damageUpgradeBtn.interactable = true;

        if (UpgradeStats.healthUpgradeCost > GameManager.Coins)
            _healthUpgradeBtn.interactable = false;
        else
            _healthUpgradeBtn.interactable = true;

        if (UpgradeStats.coinsMultiplierUpgradeCost > GameManager.Coins)
            _coinsUpgradeBtn.interactable = false;
        else
            _coinsUpgradeBtn.interactable = true;
    }

    private void HideMenu()
    {
        _raycaster.enabled = false;
        _canvasGroup.alpha = 0f;
    }
}
