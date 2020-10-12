using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Award : MonoBehaviour
{
    [SerializeField] private int _minAward = 5;
    [SerializeField] private int _maxAward = 10;
    [SerializeField] private Button _nextButton = null;

    private Button[] _chestButtons;
    public UnityEvent closeWindow = new UnityEvent();

    private void Start()
    {
        _chestButtons = GetComponentsInChildren<Button>();
    }

    public void OpenChest()
    {
        int award = Random.Range(_minAward, _maxAward) * LevelManager.LevelIndex;

        GameManager.CollectKeys(-1);
        GameManager.CollectCoins(award);
        GameObject clickedBtn = EventSystem.current.currentSelectedGameObject;
        clickedBtn.GetComponent<Button>().interactable = false;
        clickedBtn.transform.GetChild(0).GetComponent<Image>().enabled = false;
        Canvas childCanvas = clickedBtn.transform.GetComponentInChildren<Canvas>();
        childCanvas.enabled = true;
        childCanvas.GetComponentInChildren<TextMeshProUGUI>().text = string.Format("{0:# ###}", award);

        if (GameManager.Keys <= 0)
        {
            foreach (Button btn in _chestButtons)
            {
                btn.interactable = false;
            }

            _nextButton.interactable = true;
        }
    }

    public void CloseAwardWindow()
    {
        closeWindow.Invoke();
        Destroy(gameObject);
    }
}
