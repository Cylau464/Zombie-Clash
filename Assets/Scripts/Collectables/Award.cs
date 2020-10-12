using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Award : MonoBehaviour
{
    [SerializeField] private Sprite _openChestSprite = null;
    [SerializeField] private int _minAward = 5;
    [SerializeField] private int _maxAward = 10;

    private Button[] _chestButtons;

    public UnityEvent<int, bool> closeWindow;

    private void Start()
    {
        _chestButtons = GetComponentsInChildren<Button>();
        closeWindow = new UnityEvent<int, bool>();
    }

    private void OpenChest()
    {
        int award = Random.Range(_minAward, _maxAward) * LevelManager.LevelIndex;

        GameManager.CollectKeys(-1);
        GameManager.CollectCoins(award);
        GameObject clickedBtn = EventSystem.current.currentSelectedGameObject;
        clickedBtn.GetComponent<Button>().image.sprite = _openChestSprite;
        clickedBtn.GetComponent<Button>().interactable = false;
        clickedBtn.GetComponentInChildren<Image>().enabled = true;
        clickedBtn.GetComponentInChildren<TextMeshProUGUI>().text = string.Format("{0:# ###}", award);

        if (GameManager.Keys <= 0)
        {
            foreach (Button btn in _chestButtons)
            {
                btn.interactable = false;
            }
        }
    }

    private void CloseAwardWindow()
    {
        closeWindow.Invoke();
        Destroy(gameObject);
    }
}
