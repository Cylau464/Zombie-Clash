using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private int _levelIndex;

    public static LevelManager current;
    private static bool _activateNextScene;

    private UnityAction _levelCompleted;

    private void Awake()
    {
        if (current != null && current != this)
        {
            Destroy(gameObject);
            return;
        }

        current = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        TopBar.UpdateLevel(_levelIndex);
        _levelCompleted = LevelCompleted;
        GameManager.levelCompleted.AddListener(_levelCompleted);
    }

    private void LevelCompleted()
    {
        if (_levelIndex + 1 >= SceneManager.sceneCountInBuildSettings)
            _levelIndex = 0;
        else
            _levelIndex++;

        StartCoroutine(LoadScene());
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
        _activateNextScene = false;
    }
}
