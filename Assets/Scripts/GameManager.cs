using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private int _coins;
    private int _levelCoins;
    private int _keys;

    public static GameManager current;
    public static UnityEvent levelCompleted;

    private void Awake()
    {
        if (current != null && current != this)
        {
            Destroy(this);
            return;
        }

        current = this;
        levelCompleted = new UnityEvent();

        DontDestroyOnLoad(gameObject);
    }

    public static void CollectCoins(int coins)
    {
        current._levelCoins = current._coins += coins;
        TopBar.UpdateCoins(current._levelCoins);
    }

    public static void CollectKeys(int keys)
    {
        current._keys += keys;
        TopBar.UpdateKeys(current._keys);
    }

    private void LevelStart()
    {
        _levelCoins = 0;
    }
}
