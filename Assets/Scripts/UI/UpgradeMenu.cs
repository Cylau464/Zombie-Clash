using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
    public static Button damageUpgradeBtn = null;
    public static Button coinsUpgradeBtn = null;
    public static Button healthUpgradeBtn = null;

    [Header("Disable Image")]
    [SerializeField] private Image _damageDisableImage = null;
    [SerializeField] private Image _coinsDisableImage = null;
    [SerializeField] private Image _healthDisableImage = null;

    [Header("Transfusion Image")]
    [SerializeField] private Image _damageTransfusionImage = null;
    [SerializeField] private Image _coinsTransfusionImage = null;
    [SerializeField] private Image _healthTransfusionImage = null;

    [Header("Upgrade Image")]
    [SerializeField] private RectTransform _damageUpgrade = null;
    [SerializeField] private RectTransform _coinsUpgrade = null;
    [SerializeField] private RectTransform _healthUpgrade = null;
    
    [SerializeField] private float _verticalAmplitude = 20f;
    [SerializeField] private float _verticalSpeed = 1f;
    private float _startPosY;

    [Space]
    [SerializeField] private CanvasGroup _canvasGroup = null;
    [SerializeField] private GraphicRaycaster _raycaster = null;

    private void Start()
    {
        damageUpgradeBtn = _damageUpgradeBtn;
        coinsUpgradeBtn = _coinsUpgradeBtn;
        healthUpgradeBtn = _healthUpgradeBtn;

        DamageUpdate();
        HealthUpdate();
        CoinsUpdate();

        GameManager.gameStart.AddListener(HideMenu);
        UpgradeStats.damageUpgrade.AddListener(DamageUpdate);
        UpgradeStats.healthUpgrade.AddListener(HealthUpdate);
        UpgradeStats.coinsUpgrade.AddListener(CoinsUpdate);
    }

    private void Update()
    {
        float newPos = Mathf.Sin(Time.time * _verticalSpeed) * _verticalAmplitude;
        _damageUpgrade.localPosition += Vector3.up * newPos;
        _coinsUpgrade.localPosition += Vector3.up * newPos;
        _healthUpgrade.localPosition += Vector3.up * newPos;
    }

    private void DamageUpdate()
    {
        _damageLevel.text = "Lvl " + UpgradeStats.damageLevel.ToString();
        _damageCost.text = string.Format("{0:# ###}", UpgradeStats.damageUpgradeCost);
        CheckNewCost();
    }

    private void HealthUpdate()
    {
        _healthLevel.text = "Lvl " + UpgradeStats.healthLevel.ToString();
        _healthCost.text = string.Format("{0:# ###}", UpgradeStats.healthUpgradeCost);
        CheckNewCost();
    }

    private void CoinsUpdate()
    {
        _coinsMultiplierLevel.text = "Lvl " + UpgradeStats.coinsLevel.ToString();
        _coinsMultiplierCost.text = string.Format("{0:# ###}", UpgradeStats.coinsMultiplierUpgradeCost);
        CheckNewCost();
    }

    private void CheckNewCost()
    {
        if (UpgradeStats.damageUpgradeCost > GameManager.Coins)
        {
            damageUpgradeBtn.interactable = false;
            _damageDisableImage.enabled = true;
            _damageTransfusionImage.enabled = false;
            _damageUpgrade.gameObject.SetActive(false);
        }
        else
        {
            damageUpgradeBtn.interactable = true;
            _damageDisableImage.enabled = false;
            _damageTransfusionImage.enabled = true;
            _damageUpgrade.gameObject.SetActive(true);
        }

        if (UpgradeStats.healthUpgradeCost > GameManager.Coins)
        {
            healthUpgradeBtn.interactable = false;
            _healthDisableImage.enabled = true;
            _healthTransfusionImage.enabled = false;
            _healthUpgrade.gameObject.SetActive(false);
        }
        else
        {
            healthUpgradeBtn.interactable = true;
            _healthDisableImage.enabled = false;
            _healthTransfusionImage.enabled = true;
            _healthUpgrade.gameObject.SetActive(true);
        }

        if (UpgradeStats.coinsMultiplierUpgradeCost > GameManager.Coins)
        {
            coinsUpgradeBtn.interactable = false;
            _coinsDisableImage.enabled = true;
            _coinsTransfusionImage.enabled = false;
            _coinsUpgrade.gameObject.SetActive(false);
        }
        else
        {
            coinsUpgradeBtn.interactable = true;
            _coinsDisableImage.enabled = false;
            _coinsTransfusionImage.enabled = true;
            _coinsUpgrade.gameObject.SetActive(true);
        }
    }

    private void HideMenu()
    {
        _raycaster.enabled = false;
        _canvasGroup.alpha = 0f;
    }
}
