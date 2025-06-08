using Source.Scripts.Services;
using UnityEngine;
using Zenject;

namespace Source.Scripts.Ragdoll
{
  public class ForDebugOnly : MonoBehaviour
  {
#if UNITY_EDITOR
    [Inject]
    private IGameSceneService _sceneService;

    private AsyncOperation _asyncOperation;

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.Space)) 
        _sceneService.ReloadGame();
    }
#endif
  }
}