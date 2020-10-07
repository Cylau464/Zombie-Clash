using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEditor.PackageManager;

public class TopBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinText = null;
    [SerializeField] private Slider _keySlider = null;
    [SerializeField] private TextMeshProUGUI _levelNumberText = null;
    [SerializeField] private Slider _levelProgressSlider = null;

    public static TopBar current;

    private void Awake()
    {
        if (current != null && current != this)
        {
            Destroy(transform.parent.gameObject);
            return;
        }

        current = this;

        _coinText.text = "0";
        _keySlider.value = 0;
        _levelNumberText.text = "УРОВЕНЬ 0";
        _levelProgressSlider.value = 0;
    }

    public static void UpdateLevel(int level)
    {
        current._levelNumberText.text = "УРОВЕНЬ " + level.ToString();
    }

    public static void UpdateCoins(int coins)
    {
        current._coinText.text = coins.ToString();
    }

    public static void UpdateKeys(int keys)
    {
        current._keySlider.value = keys;
    }

    public static void UpdateLevelProgress(float progress)
    {
        current._levelProgressSlider.value = progress;
    }
}
