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

        AsyncOperation loadLevel = SceneManager.LoadSceneAsync(_levelIndex);
        loadLevel.allowSceneActivation = false;

        while (!loadLevel.isDone)
        {
            // Check if the load has finished
            if (loadLevel.progress >= 0.9f)
            {
                if (_activateNextScene)
                    //Activate the Scene
                    loadLevel.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
