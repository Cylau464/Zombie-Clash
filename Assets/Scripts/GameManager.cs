using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int _coins;
    private int _levelCoins;
    private int _keys;
    private int _soldiersCount;

    public static int Coins
    {
        get { return current._coins; }
    }
    public int SolidersCount
    {
        get { return _soldiersCount; }
        set
        {
            if (value <= 0 && _soldiersCount > 0)
                GameOver();

            _soldiersCount = Mathf.Clamp(value, 0, int.MaxValue);
        }
    }

    public static GameManager current;
    public static UnityEvent levelCompleted;
    public static UnityEvent gameStart;

    private void Awake()
    {
        if (current != null && current != this)
        {
            Destroy(gameObject);
            return;
        }

        current = this;
        gameStart = new UnityEvent();
        levelCompleted = new UnityEvent();
        levelCompleted.AddListener(LevelCompleted);

        DontDestroyOnLoad(gameObject);
    }

    public static void CollectCoins(int coins)
    {
        current._levelCoins = current._coins += coins;
        TopBar.UpdateCoins(current._coins);
    }

    public static void CollectKeys(int keys)
    {
        current._keys += keys;
        TopBar.UpdateKeys(current._keys);
    }

    private void LevelCompleted()
    {
        LevelEndMenu.activateMenuEvent.Invoke(_levelCoins, false);
        InputController.movementAvailable = false;
    }

    private void OnLevelWasLoaded(int level)
    {
        if (current != null && current != this) return;

        _levelCoins = 0;
        _soldiersCount = 0;
        TopBar.UpdateCoins(current._coins);
        TopBar.UpdateKeys(current._keys);
    }

    public void StartGame()
    {
        InputController.movementAvailable = true;
        gameStart.Invoke();
    }

    private void GameOver()
    {
        InputController.movementAvailable = false;
        LevelEndMenu.activateMenuEvent.Invoke(_levelCoins, true);
    }

    public void Restart()
    {
        InputController.movementAvailable = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
