using System.Collections;
using UnityEngine;

namespace Source.Scripts.Infrastructutre
{
  public class CoroutineInvoker : MonoBehaviour
  {
    private static CoroutineInvoker _instance;

    public static CoroutineInvoker Instance
    {
      get
      {
        if (_instance) 
          return _instance;
        
        GameObject coroutinesGameObject = new GameObject("CoroutineInvoker");
        _instance = coroutinesGameObject.AddComponent<CoroutineInvoker>();
        DontDestroyOnLoad(coroutinesGameObject);

        return _instance;
      }
    }

    public Coroutine StartRoutine(IEnumerator enumerator) => 
      StartCoroutine(enumerator);

    public void StopRoutine(Coroutine routine) => 
      StopCoroutine(routine);
  }
}