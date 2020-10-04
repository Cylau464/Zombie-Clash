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
    private UnityAction _levelCompletedAction;
    public static bool gameIsStart;

    private void OnGUI()
    {
        GUI.Box(new Rect(new Vector2(20f, 20f), new Vector2(100f, 20f)), _soldiersCount.ToString());
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
        levelCompleted = new UnityEvent();
        _levelCompletedAction = LevelCompleted;
        levelCompleted.AddListener(_levelCompletedAction);

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
        //LevelEndMenu.ActivateMenu(_levelCoins, false);
        LevelEndMenu.activateMenuEvent.Invoke(_levelCoins, false);
        InputController.movementAvailable = false;
    }

    private void OnLevelWasLoaded(int level)
    {
        if (current != null && current != this) return;

        _levelCoins = 0;
        //current.SolidersCount = 0;
        _soldiersCount = 0;
        TopBar.UpdateCoins(current._coins);
        TopBar.UpdateKeys(current._keys);

        if(gameIsStart)
            InputController.movementAvailable = true;
    }

    public void StartGame()
    {
        if (gameIsStart) return;

        InputController.movementAvailable = true;
        gameStart.Invoke();
        gameIsStart = true;
    }

    private void GameOver()
    {
        InputController.movementAvailable = false;
        LevelEndMenu.activateMenuEvent.Invoke(_levelCoins, true);
    }

    public void Restart()
    {        
        //InputController.movementAvailable = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        InputController.movementAvailable = true;
    }
}
