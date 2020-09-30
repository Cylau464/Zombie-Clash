using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TopBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinText = null;
    [SerializeField] private TextMeshProUGUI _keyText = null;
    [SerializeField] private TextMeshProUGUI _levelNumberText = null;

    public static TopBar current;

    private void Awake()
    {
        if (current != null && current != this)
        {
            Destroy(this);
            return;
        }

        current = this;

        DontDestroyOnLoad(gameObject);
    }

    public static void UpdateLevel(int level)
    {
        current._levelNumberText.text = level.ToString() + " уровень";
    }

    public static void UpdateCoins(int coins)
    {
        current._coinText.text = coins.ToString();
    }

    public static void UpdateKeys(int keys)
    {
        current._keyText.text = keys.ToString();
    }
}
