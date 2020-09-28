using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private int _levelIndex;

    public static LevelManager current;

    private UnityAction _levelCompleted;
    private UnityAction _switchLevel;
    private bool _activateNextScene;

    private void Awake()
    {
        if (current != null && current != this)
        {
            Destroy(this);
            return;
        }

        current = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _levelCompleted = LevelCompleted;
        GameManager.levelCompleted.AddListener(_levelCompleted);
        _switchLevel = SwitchLevel;
        //Addlistener to button
    }

    private void LevelCompleted()
    {
        _levelIndex++;
        StartCoroutine(LoadScene());
    }

    private void SwitchLevel()
    {
        _activateNextScene = true;
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
}
