using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private int _levelIndex = 1;
    private int _levelDefenders;

    public static int LevelIndex
    {
        get { return current._levelIndex; }
    }

    public static LevelManager current;
    private static bool _activateNextScene;

    private void Awake()
    {
        if (current != null && current != this)
        {
            Destroy(gameObject);
            return;
        }

        current = this;

        DontDestroyOnLoad(gameObject);

        SaveData saveData = SaveSystem.LoadData();

        if (saveData != null)
            _levelIndex = saveData.levelIndex;

        SceneManager.LoadScene(_levelIndex);
    }

    private void Start()
    {
        GameManager.levelCompleted.AddListener(LevelCompleted);
    }

    private void LevelCompleted()
    {
        if (_levelIndex + 1 >= SceneManager.sceneCountInBuildSettings)
            _levelIndex = 1;
        else
            _levelIndex++;

        StartCoroutine(LoadScene());

        SaveSystem.SaveData();
    }

    public void SwitchLevel()
    {
        _activateNextScene = true;
        TopBar.UpdateLevel(_levelIndex);
    }

    private IEnumerator LoadScene()
    {
        yield return null;
        
        AsyncOperation loadingLevel = SceneManager.LoadSceneAsync(_levelIndex);
        loadingLevel.allowSceneActivation = false;

        while (!loadingLevel.isDone)
        {
            // Check if the load has finished
            if (loadingLevel.progress >= 0.9f)
            {
                if (_activateNextScene)
                    //Activate the Scene
                    loadingLevel.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        TopBar.UpdateLevel(_levelIndex);
        _activateNextScene = false;
    }
}
