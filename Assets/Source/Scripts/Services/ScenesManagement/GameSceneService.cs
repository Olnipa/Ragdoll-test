using System;
using Zenject;

namespace Source.Scripts.Services
{
  public class GameSceneService : IGameSceneService
  {
    private const string CoreGameScene = "GamePlayScene";

    private readonly SceneLoader _sceneLoader;

    [Inject]
    public GameSceneService(SceneLoader sceneLoader) => 
      _sceneLoader = sceneLoader;

    public void LoadMainGameScene(Action onLoad, float minWaitingTime = 0) => 
      _sceneLoader.Load(CoreGameScene, minWaitingTime, onLoad);
  }
}