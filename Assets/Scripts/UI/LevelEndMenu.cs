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

    [SerializeField] private Image _loseImage = null;
    [SerializeField] private Image _winImage = null;

    [SerializeField] private GameObject _awardWindowPrefab = null;

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
        _loseImage.enabled = false;
        _winImage.enabled = false;
        current._canvas.alpha = 0f;

        activateMenuEvent = new UnityEvent<int, bool>();
        activateMenuEvent.AddListener(ActivateMenu);
    }

    public void ActivateMenu(int coins, bool defeat)
    {
        current._canvas.alpha = 1f;

        if (GameManager.Keys >= 3)
        {
            GameObject go = Instantiate(_awardWindowPrefab, transform.position, Quaternion.identity, transform);
            go.GetComponent<Award>().closeWindow.AddListener(() => ShowMenu(coins, defeat));
        }
        else
        {
            ShowMenu(coins, defeat);
        }
    }

    private void ShowMenu(int coins, bool defeat)
    {
        if (defeat)
        {
            current._restartBtn.gameObject.SetActive(true);
            current._loseImage.enabled = true;
        }
        else
        {
            current._nextLevelBtn.gameObject.SetActive(true);
            current._winImage.enabled = true;
        }

        current._receivedCoinsText.text = "+" + string.Format("{0:# ###}", coins);
    }
}
