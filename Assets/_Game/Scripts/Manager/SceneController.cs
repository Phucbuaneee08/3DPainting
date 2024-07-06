using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneController : Singleton<SceneController>
{
    public string currentSceneName = "Loading";
    public bool isLoadingNewScene = false;
    public void LoadCurrentScene(Action OnComplete = null)
    {
        ChangeScene(SceneManager.GetActiveScene().name, OnComplete);
        currentSceneName = SceneManager.GetActiveScene().name;
    }

    public void ChangeScene(string sceneName, Action OnComplete)
    {
        if (isLoadingNewScene) return;
        StartCoroutine(I_ChangeScene(sceneName, OnComplete));
    }
    IEnumerator I_ChangeScene(string sceneName, Action OnComplete)
    {
        isLoadingNewScene = true;
        SimplePool.CollectAll();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        Scene scene = SceneManager.GetSceneByName(sceneName);
        yield return new WaitUntil(() => scene.IsValid() && scene.isLoaded);
        currentSceneName = sceneName;
        OnComplete?.Invoke();
        isLoadingNewScene = false;
        yield return null;
    }

}
