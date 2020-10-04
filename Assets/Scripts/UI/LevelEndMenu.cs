﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelEndMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _receivedCoinsText = null;
    [SerializeField] private CanvasGroup _canvas = null;
    [SerializeField] private Button _nextLevelBtn = null;
    [SerializeField] private Button _restartBtn = null;

    public static LevelEndMenu current;
    public static UnityEvent<int, bool> activateMenuEvent;

    private void Awake()
    {
        if (current != null && current != this)
        {
            Destroy(this);
            return;
        }

        current = this;

        _nextLevelBtn.gameObject.SetActive(false);
        _restartBtn.gameObject.SetActive(false);
        current._canvas.alpha = 0f;

        activateMenuEvent = new UnityEvent<int, bool>();
        activateMenuEvent.AddListener(ActivateMenu);
    }

    public void ActivateMenu(int coins, bool defeat)
    {
        if (defeat)
            current._restartBtn.gameObject.SetActive(true);
        else
            current._nextLevelBtn.gameObject.SetActive(true);

        current._canvas.alpha = 1f;
        current._receivedCoinsText.text = "+" + coins.ToString();
    }
}
