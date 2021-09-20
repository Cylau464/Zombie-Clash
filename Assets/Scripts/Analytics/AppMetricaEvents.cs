using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AppMetricaEvents : MonoBehaviour
{
    private const string LEVEL_START_EVENT = "level_start";
    private const string LEVEL_FINISH_EVENT = "level_finish";
    private const string KEY = "level";

    private Dictionary<string, object> _eventParameters = new Dictionary<string, object>();

    private static AppMetricaEvents Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        GameManager.gameStart.AddListener(LevelStart);
        GameManager.gameOver.AddListener(() => LevelFinish(false));
        GameManager.levelCompleted.AddListener(() => LevelFinish(true));
    }

    private void LevelStart()
    {
        _eventParameters[KEY] = LevelManager.LevelNumber;
        AppMetrica.Instance.ReportEvent(LEVEL_START_EVENT, _eventParameters);
        AppMetrica.Instance.SendEventsBuffer();
        _eventParameters.Clear();
    }

    private void LevelFinish(bool victory)
    {
        string result = victory ? "Win" : "Lose";
        _eventParameters = new Dictionary<string, object>()
        {
            { "level",  LevelManager.LevelNumber },
            { "result", result },
            { "time", Mathf.RoundToInt(GameManager.timeSpentOnLevel) },
            { "progress", Mathf.FloorToInt(LevelProgressHandler.Progress * 100f) }
        };
        AppMetrica.Instance.ReportEvent(LEVEL_FINISH_EVENT, _eventParameters);
        AppMetrica.Instance.SendEventsBuffer();
        _eventParameters.Clear();
    }
}