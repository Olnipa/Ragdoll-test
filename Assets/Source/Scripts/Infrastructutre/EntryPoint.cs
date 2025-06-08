using Source.Scripts.Services;
using UnityEngine;
using Zenject;

namespace Source.Scripts.Infrastructutre
{
  public class EntryPoint : MonoBehaviour
  {
    [SerializeField] private float _minLoadingTime = 2;
    
    private IGameSceneService _sceneService;

    [Inject]
    public void Construct(IGameSceneService sceneService) => 
      _sceneService = sceneService;

    public void Start() => 
      _sceneService.LoadMainGameScene(null, _minLoadingTime);
  }
}