using System.Collections;
using System.Collections.Generic;
using System.Data;
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
        private set { current._coins = value; }
    }

    public static int Keys
    { 
        get { return current._keys; }
        private set
        {
            current._keys = value > 3 ? 3 : value; 
        }
    }

    public int SolidersCount
    {
        get { return _soldiersCount; }
        set
        {
            //Debug.Log(value + " VALUE SOLD COUNT " + _soldiersCount);
            if (value <= 0 && _soldiersCount > 0)
                gameOver.Invoke();

            _soldiersCount = Mathf.Clamp(value, 0, int.MaxValue);
        }
    }

    public static GameManager current;
    public static UnityEvent levelCompleted;
    public static UnityEvent gameStart;
    public static UnityEvent gameOver;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            SaveSystem.DeleteSave();
            Debug.Log("The save has been deleted!");
        }
        if (Input.GetKeyDown(KeyCode.A))
            Coins += 10000;

        if(Input.GetKeyDown(KeyCode.S))
        {
            levelCompleted.Invoke();
        }
    }

    private void Awake()
    {
        if (current != null && current != this)
        {
            Destroy(gameObject);
            return;
        }

        current = this;
        gameStart = new UnityEvent();
        gameOver = new UnityEvent();
        gameOver.AddListener(GameOver);
        levelCompleted = new UnityEvent();
        levelCompleted.AddListener(LevelCompleted);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SaveData saveData = SaveSystem.LoadData();

        if (saveData != null)
        {
            Coins = saveData.coins;
            Keys = saveData.keys;
            TopBar.UpdateCoins(current._coins);
            TopBar.UpdateKeys(current._keys);
            UpgradeStats.LoadStats(saveData);
            FightStage.bossHealthLeft = saveData.bossHealthLeft;
            FightStage.defendersLeft = saveData.defendersLeft;
            CollectableObjectContainer.keys = saveData.collectedKeys;
        }
    }

    public static void CollectCoins(int coins)
    {
        if(coins > 0)
            current._levelCoins += coins;

        current._coins += coins;
        TopBar.UpdateCoins(current._coins);

        SaveSystem.SaveData();
    }

    public static void CollectKeys(int keys)
    {
        Keys += keys;
        TopBar.UpdateKeys(current._keys);

        SaveSystem.SaveData();
    }

    private void LevelCompleted()
    {
        int awardCoins = GetLevelAwardCoins(true);
        Coins += awardCoins;
        LevelEndMenu.activateMenuEvent.Invoke(_levelCoins + awardCoins, false);
        InputController.movementAvailable = false;
        AudioManager.PlayWinSound();
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
        int awardCoins = GetLevelAwardCoins(false);
        Coins += awardCoins;
        InputController.movementAvailable = false;
        LevelEndMenu.activateMenuEvent.Invoke(_levelCoins + awardCoins, true);

        if(FightStage.fightBegan)
            AudioManager.PlayLoseSound();

        SaveSystem.SaveData();
    }

    public void Restart()
    {
        InputController.movementAvailable = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private int GetLevelAwardCoins(bool victory)
    {
        if (victory)
            return Mathf.FloorToInt(LevelManager.LevelNumber * UpgradeStats.coinsMultiplier * 10);
        else
            return Mathf.FloorToInt(LevelManager.LevelNumber * UpgradeStats.coinsMultiplier * 10) / 2;
    }
}
