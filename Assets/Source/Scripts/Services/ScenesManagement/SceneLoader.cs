using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Source.Scripts.Services
{
  public class SceneLoader
  {
    private readonly CoroutineInvoker _coroutineInvoker;

    [Inject]
    public SceneLoader(CoroutineInvoker coroutineInvoker) => 
      _coroutineInvoker = coroutineInvoker;

    public void Load(string nextScene, float minWaitingTime = 0, Action onLoaded = null) => 
      _coroutineInvoker.StartRoutine(LoadScene(nextScene, minWaitingTime, onLoaded));

    private IEnumerator LoadScene(string name, float minWaitTime, Action onLoaded = null)
    {
      if (SceneManager.GetActiveScene().name == name)
      {
        onLoaded?.Invoke();
        yield break;
      }
            
      AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(name);
      waitNextScene.allowSceneActivation = false;

      while (waitNextScene.progress < 0.9f || minWaitTime > 0)
      {
        minWaitTime -= Time.deltaTime;
        yield return null;
      }
      
      waitNextScene.allowSceneActivation = true;
      onLoaded?.Invoke();
    }
  }
}