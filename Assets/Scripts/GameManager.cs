using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private int _coins;
    private int _levelCoins;

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
        //ProgressBar.UpdateCoins(current._coins);
    }

    private void LevelStart()
    {
        _levelCoins = 0;
    }
}
