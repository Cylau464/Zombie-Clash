using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private int _sceneIndex = 1;
    private int _levelNumber = 1;
    private int _levelDefenders;

    public static int LevelNumber
    {
        get { return current._levelNumber; }
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
            _levelNumber = saveData.levelNumber;

        // -1 because first scene (0) is empty
        int sceneCount = (SceneManager.sceneCountInBuildSettings - 1);
        int cycleNumber = Mathf.FloorToInt((_levelNumber - 1) / sceneCount);

        if (cycleNumber > 0 && _levelNumber != sceneCount)
            _sceneIndex = _levelNumber - cycleNumber * sceneCount;
        else
            _sceneIndex = _levelNumber;

        SceneManager.LoadScene(_sceneIndex);
    }

    private void Start()
    {
        GameManager.levelCompleted.AddListener(LevelCompleted);
    }

    private void LevelCompleted()
    {
        if (_sceneIndex + 1 >= SceneManager.sceneCountInBuildSettings)
            _sceneIndex = 1;
        else
            _sceneIndex++;

        _levelNumber++;

        StartCoroutine(LoadScene());

        SaveSystem.SaveData();
    }

    public void SwitchLevel()
    {
        _activateNextScene = true;
        TopBar.UpdateLevel(LevelNumber);
    }

    private IEnumerator LoadScene()
    {
        yield return null;
        
        AsyncOperation loadingLevel = SceneManager.LoadSceneAsync(_sceneIndex);
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
        if (current != null && current != this) return;

        TopBar.UpdateLevel(_levelNumber);
        _activateNextScene = false;
    }
}
