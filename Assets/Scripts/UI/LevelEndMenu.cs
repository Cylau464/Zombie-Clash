using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelEndMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _receivedCoinsText = null;
    [SerializeField] private CanvasGroup _canvas = null;

    public static LevelEndMenu current;

    private void Awake()
    {
        if (current != null && current != this)
        {
            Destroy(this);
            return;
        }

        current = this;

        current._canvas.alpha = 0f;
    }

    public static void ActivateMenu(int coins)
    {
        current._canvas.alpha = 1f;
        current._receivedCoinsText.text = "+" + coins.ToString();
    }

    public void DeactivateMenu()
    {
        current._canvas.alpha = 0f;
    }
}
